using Denifia.Stardew.SendItems.Domain;
using Denifia.Stardew.SendItems.Events;
using Denifia.Stardew.SendItems.Framework;
using RestSharp;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendItems.Services
{
    public interface IMailCleanupService
    {
    }

    public class MailCleanupService : IMailCleanupService
    {
        private readonly IMod _mod;
        private readonly IConfigurationService _configService;
        private readonly IFarmerService _farmerService;
        private RestClient _restClient { get; set; }

        public MailCleanupService(IMod mod, IConfigurationService configService, IFarmerService farmerService)
        {
            _mod = mod;
            _configService = configService;
            _farmerService = farmerService;
            _restClient = new RestClient(_configService.GetApiUri());

            ModEvents.OnMailCleanup += OnMailCleanup;
            ModEvents.MailDelivered += MailDelivered;
        }

        private void OnMailCleanup(object sender, EventArgs e)
        {
            DeleteFutureComposedMail();
            UnreadFutureReadMail();
            Task.Run(DeleteReadMail);

            ModEvents.RaiseOnMailDelivery(this, EventArgs.Empty);
        }

        private async Task DeleteReadMail()
        {
            var localMail = Repository.Instance.Fetch<Mail>(x =>
                x.Status == MailStatus.Read &&
                x.ToFarmerId == _farmerService.CurrentFarmer.Id
            );
            if (!localMail.Any()) return;

            var logPrefix = "[CleanRead] ";
            _mod.Monitor.Log($"{logPrefix}Clean up read cloud mail...", LogLevel.Debug);
            var currentGameDateTime = ModHelper.GetGameDayTime();
            var readMail = localMail.Where(x => x.ReadInGameDate != null && x.ReadInGameDate < currentGameDateTime).ToList();
            if (readMail.Any())
            {
                _mod.Monitor.Log($"{logPrefix}.clearing {readMail.Count} read mail...", LogLevel.Debug);
                foreach (var mail in readMail)
                {
                    await DeleteRemoteMail(mail, logPrefix);
                }
            }
            _mod.Monitor.Log($"{logPrefix}.done", LogLevel.Debug);
        }

        private void DeleteFutureComposedMail()
        {
            var localMail = Repository.Instance.Fetch<Mail>(x => 
                x.Status == MailStatus.Composed && 
                x.ToFarmerId == _farmerService.CurrentFarmer.Id
            );
            if (!localMail.Any()) return;
            var currentGameDateTime = ModHelper.GetGameDayTime();
            var futureMail = localMail.Where(x => x.CreatedInGameDate > currentGameDateTime).ToList();
            foreach (var mail in futureMail)
            {
                Repository.Instance.Delete<Mail>(x => x.Id == mail.Id);
            }
        }

        private void UnreadFutureReadMail()
        {
            var localMail = Repository.Instance.Fetch<Mail>(x =>
                x.Status == MailStatus.Read &&
                x.ToFarmerId == _farmerService.CurrentFarmer.Id
            );
            if (!localMail.Any()) return;
            var currentGameDateTime = ModHelper.GetGameDayTime();
            var futureMail = localMail.Where(x => x.ReadInGameDate == null || x.ReadInGameDate > currentGameDateTime).ToList();
            foreach (var mail in futureMail)
            {
                mail.Status = MailStatus.Delivered;
                mail.ReadInGameDate = null;
            }

            if (!futureMail.Any()) return;
            Repository.Instance.Upsert(futureMail.AsEnumerable());
        }

        private async void MailDelivered(object sender, EventArgs e)
        {
            try
            {
                await DeleteDeliveredRemoteMail();
            }
            catch (Exception ex)
            {
                ModHelper.HandleError(_mod, ex, "deleting mail from server");
            }
        }

        private async Task DeleteDeliveredRemoteMail()
        {
            var logPrefix = "[CleanDelivered] ";
            _mod.Monitor.Log($"{logPrefix}Clean up delivered cloud mail...", LogLevel.Debug);
            var localMail = Repository.Instance.Fetch<Mail>(x => x.Status == MailStatus.Delivered);
            if (localMail.Any())
            {
                _mod.Monitor.Log($"{logPrefix}.clearing {localMail.Count} delivered mail...", LogLevel.Debug);
                foreach (var mail in localMail)
                {
                    await DeleteRemoteMail(mail, logPrefix);
                }
            }
            _mod.Monitor.Log($"{logPrefix}.done", LogLevel.Debug);
        }

        private async Task DeleteRemoteMail(Mail mail, string logPrexix)
        {
            var urlSegments = new Dictionary<string, string> { { "mailId", mail.Id.ToString() } };
            var request = ModHelper.FormStandardRequest("mail/{mailId}", urlSegments, Method.DELETE);
            var response = await _restClient.ExecuteTaskAsync<bool>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _mod.Monitor.Log($"{logPrexix}..done", LogLevel.Debug);
                // all good :)
            }
        }
    }
}
