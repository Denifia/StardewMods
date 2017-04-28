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
        }

        private void DeleteFutureComposedMail()
        {
            var localMail = Repository.Instance.Fetch<Mail>(x => 
                x.Status == MailStatus.Composed && 
                x.ToFarmerId == _farmerService.CurrentFarmer.Id
            );
            var currentGameDateTime = ModHelper.GetGameDayTime();
            var futureMail = localMail.Where(x => x.CreatedInGameDate > currentGameDateTime).ToList();
            Repository.Instance.Delete<Mail>(x => futureMail.Contains(x));
        }

        private void UnreadFutureReadMail()
        {
            var localMail = Repository.Instance.Fetch<Mail>(x =>
                x.Status == MailStatus.Read &&
                x.ToFarmerId == _farmerService.CurrentFarmer.Id
            );
            var currentGameDateTime = ModHelper.GetGameDayTime();
            var futureMail = localMail.Where(x => x.ReadInGameDate > currentGameDateTime).ToList();
            foreach (var mail in futureMail)
            {
                mail.Status = MailStatus.Delivered;
                mail.ReadInGameDate = null;
            }
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
                ModHelper.HandleError(_mod, ex, "delivering mail on schedule");
            }
        }

        private async Task DeleteDeliveredRemoteMail()
        {
            var localMail = Repository.Instance.Fetch<Mail>(x => x.Status == MailStatus.Delivered);
        }
    }
}
