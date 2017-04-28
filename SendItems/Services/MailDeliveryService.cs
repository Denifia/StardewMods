using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Denifia.Stardew.SendItems.Domain;
using RestSharp;
using Denifia.Stardew.SendItems.Events;
using StardewModdingAPI.Events;
using Denifia.Stardew.SendItems.Models;
using StardewValley;
using Denifia.Stardew.SendItems.Framework;
using StardewModdingAPI;

namespace Denifia.Stardew.SendItems.Services
{
    public interface IMailDeliveryService
    {
    }

    /// <summary>
    /// Handles the local and remote delivery of mail to farmers
    /// </summary>
    public class MailDeliveryService : IMailDeliveryService
    {
        private readonly IMod _mod;
        private readonly IConfigurationService _configService;
        private readonly IFarmerService _farmerService;
        private RestClient _restClient { get; set; }

        public MailDeliveryService(IMod mod, IConfigurationService configService, IFarmerService farmerService)
        {
            _mod = mod;
            _configService = configService;
            _farmerService = farmerService;
            _restClient = new RestClient(_configService.GetApiUri());

            TimeEvents.AfterDayStarted += AfterDayStarted;
            TimeEvents.TimeOfDayChanged += TimeOfDayChanged;
            ModEvents.OnMailDeliverySchedule += OnMailDeliverySchedule;
        }

        private async void OnMailDeliverySchedule(object sender, EventArgs e)
        {
            try
            {
                await DeliverPostedMail();
            }
            catch (Exception ex)
            {
                ModHelper.HandleError(_mod, ex, "delivering mail on schedule");
            }
        }

        private async Task DeliverPostedMail()
        {
            DeliverLocalMail();
            if (!_configService.InLocalOnlyMode())
            {
                await DeliverLocalMailToCloud();
                await DeliverCloudMailLocally();
            }
            DeliverMailToLetterBox();
        }

        private void DeliverMailToLetterBox()
        {
            if (_farmerService.CurrentFarmer == null) return;
            var currentFarmerId = _farmerService.CurrentFarmer.Id;

            var count = Repository.Instance.Fetch<Mail>(x => x.Status == MailStatus.Delivered && x.ToFarmerId == currentFarmerId).Count;
            if (count > 0)
            {
                while (Game1.mailbox.Any() && Game1.mailbox.Peek() == ModConstants.PlayerMailKey)
                {
                    Game1.mailbox.Dequeue();
                }

                for (int i = 0; i < count; i++)
                {
                    Game1.mailbox.Enqueue(ModConstants.PlayerMailKey);
                }
            }
        }

        private void DeliverLocalMail()
        {
            var localMail = GetLocallyComposedMail();
            var localFarmers = _farmerService.GetFarmers();
            var updatedLocalMail = new List<Mail>();

            foreach (var mail in localMail)
            {
                if (localFarmers.Any(x => x.Id == mail.ToFarmerId))
                {
                    mail.Status = MailStatus.Delivered;
                    updatedLocalMail.Add(mail);
				}
            }

            UpdateLocalMail(updatedLocalMail);
            ModEvents.RaiseMailDelivered(this, EventArgs.Empty);
        }

        private async Task DeliverLocalMailToCloud()
        {
            var localMail = GetLocallyComposedMail();
            var localFarmers = _farmerService.GetFarmers();
            var updatedLocalMail = new List<Mail>();

            // Consider: Add an api method that takes a list of MailCreateModels
            foreach (var mail in localMail)
            {
                if (!localFarmers.Any(x => x.Id == mail.ToFarmerId))
                {
                    var mailCreateModel = new MailCreateModel
                    {
                        ToFarmerId = mail.ToFarmerId,
                        FromFarmerId = mail.FromFarmerId,
                        Text = mail.Text,
                        CreatedDate = mail.CreatedDate
                    };

                    var urlSegments = new Dictionary<string, string> { { "mailId", mail.Id.ToString() } };
                    var request = FormStandardRequest("mail/{mailId}", urlSegments, Method.PUT);
                    var response = await _restClient.ExecuteTaskAsync<bool>(request);

                    if (response.Data)
                    {
                        mail.Status = MailStatus.Posted;
                        updatedLocalMail.Add(mail);
                    }
                }
            }
        }

        private async Task DeliverCloudMailLocally()
        {
            var remoteMail = await GetRemotelyPostedMailForCurrentFarmerAsync();
            if (!remoteMail.Any()) return;
            
            var localFarmers = _farmerService.GetFarmers();
            if (!localFarmers.Any()) return;

            var localFarmer = localFarmers.FirstOrDefault(x => x.Id == remoteMail.First().ToFarmerId);
            if (localFarmer == null) return;

            var localMail = Repository.Instance.Fetch<Mail>(x => x.ToFarmerId == localFarmer.Id);
            var mailNotLocal = remoteMail.Where(x => !localMail.Contains(x)).ToList();
            foreach (var mail in mailNotLocal)
            {
                mail.Status = MailStatus.Delivered;
            }

            Repository.Instance.Upsert(mailNotLocal.AsEnumerable());
        }

        private List<Mail> GetLocallyComposedMail()
        {
            return Repository.Instance.Fetch<Mail>(x => x.Status == MailStatus.Composed);
        }

        private async Task<List<Mail>> GetRemotelyPostedMailForCurrentFarmerAsync()
        {
            if (_farmerService.CurrentFarmer == null) return new List<Mail>();
            var currentFarmerId = _farmerService.CurrentFarmer.Id;

            var urlSegments = new Dictionary<string, string> { { "farmerId", currentFarmerId } };
			var request = FormStandardRequest("mail/to/{farmerId}", urlSegments, Method.GET);
            var response = await _restClient.ExecuteTaskAsync<List<Mail>>(request);

            var mail = new List<Mail>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (response.Data != null && response.Data.GetType() == typeof(List<Mail>))
                {
                    mail.AddRange(response.Data);
                }
            }

            return mail;
        }

        private void AfterDayStarted(object sender, EventArgs e)
        {
            // Deliver mail each night
            ModEvents.RaiseOnMailDeliverySchedule(this, EventArgs.Empty);
        }

        private void TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            var timeToCheck = false;

            // Deliver mail at lunch time
            if (e.NewInt == 1200)
            {
                timeToCheck = true;
            }

            if (timeToCheck || _configService.InDebugMode())
            {
                ModEvents.RaiseOnMailDeliverySchedule(this, EventArgs.Empty);
            }
        }

        private void UpdateLocalMail(List<Mail> mail)
        {
            if (!mail.Any()) return;
            Repository.Instance.Update(mail.AsEnumerable());
        }

        private RestRequest FormStandardRequest(string resource, Dictionary<string, string> urlSegments, Method method)
		{
			var request = new RestRequest(resource, method);
			request.AddHeader("Content-type", "application/json; charset=utf-8");
			foreach (var urlSegment in urlSegments)
			{
				request.AddUrlSegment(urlSegment.Key, urlSegment.Value);
			}
			return request;
		}
    }
}
