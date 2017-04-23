using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Denifia.Stardew.SendItems.Domain;
using LiteDB;
using RestSharp;
using Denifia.Stardew.SendItems.Events;
using StardewModdingAPI.Events;
using Denifia.Stardew.SendItems.Models;

namespace Denifia.Stardew.SendItems.Services
{
    public interface IMailDeliveryService
    {
        Task DeliverPostedMail();
    }

    /// <summary>
    /// Handles the local and remote delivery of mail to farmers
    /// </summary>
    public class MailDeliveryService : IMailDeliveryService
    {

        private IConfigurationService _configService;
        private IFarmerService _farmerService;
        private RestClient _restClient { get; set; }

        public MailDeliveryService(IConfigurationService configService, IFarmerService farmerService)
        {
            _configService = configService;
            _farmerService = farmerService;
            _restClient = new RestClient(_configService.GetApiUri());

            TimeEvents.AfterDayStarted += AfterDayStarted;
            TimeEvents.TimeOfDayChanged += TimeOfDayChanged;
            SendItemsModEvents.OnMailDeliverySchedule += OnMailDeliverySchedule;
        }

        private void OnMailDeliverySchedule(object sender, EventArgs e)
        {
            Task.Run(DeliverPostedMail).Wait();
        }

        public async Task DeliverPostedMail()
        {
            await DeliverLocalMail();
            if (_configService.InLocalOnlyMode())
            {
                await DeliverLocalMailToCloud();
                await DeliverCloudMailLocally();
            }
        }

        private async Task DeliverLocalMail()
        {
            var localMail = await GetLocallyComposedMail();
            var localFarmers = await _farmerService.GetFarmersAsync();
            var updatedLocalMail = new List<Mail>();

            foreach (var mail in localMail)
            {
                if (localFarmers.Any(x => x.Id == mail.ToFarmerId))
                {
                    mail.Status = MailStatus.Delivered;
                    updatedLocalMail.Add(mail);
				}
            }

            await UpdateLocalMail(updatedLocalMail);
        }

        private async Task DeliverLocalMailToCloud()
        {
            var localMail = await GetLocallyComposedMail();
            var localFarmers = await _farmerService.GetFarmersAsync();
            var updatedLocalMail = new List<Mail>();

            foreach (var mail in localMail)
            {
                if (!localFarmers.Any(x => x.Id == mail.ToFarmerId))
                {
                    var mailCreateModel = new MailCreateModel
                    {
                        ToFarmerId = mail.ToFarmerId,
                        FromFarmerId = mail.FromFarmerId,
                        Text = mail.Text
                    };

                    var urlSegments = new Dictionary<string, string>();
                    var request = FormStandardRequest("mail", urlSegments, Method.GET);
                    var response = _restClient.Execute<Guid>(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        mail.Status = MailStatus.Posted;
                        updatedLocalMail.Add(mail);
                    }
                }
            }
        }

        private async Task DeliverCloudMailLocally()
        {
            var remoteMail = await GetRemotelyPostedMailForCurrentFarmer();
            if (!remoteMail.Any()) return;

            var localFarmers = await _farmerService.GetFarmersAsync();
            if (!localFarmers.Any()) return;

            var localFarmer = localFarmers.FirstOrDefault(x => x.Id == remoteMail.First().ToFarmerId);
            if (localFarmer == null) return;

            foreach (var mail in remoteMail)
            {
                mail.Status = MailStatus.Delivered;
            }

            using (var db = new LiteRepository(_configService.ConnectionString))
            {   
                db.Update(remoteMail);
            }
        }

        private async Task<List<Mail>> GetLocallyComposedMail()
        {
            return await Task.Run(() =>
            {
                using (var db = new LiteRepository(_configService.ConnectionString))
                {
                    return db.Query<Mail>().Where(x => x.Status == MailStatus.Composed).ToList();
                }
            });
        }

        private async Task<List<Mail>> GetRemotelyPostedMailForCurrentFarmer()
        {
            return await Task.Run(() =>
            {
                var urlSegments = new Dictionary<string, string> { { "farmerId", _farmerService.CurrentFarmer.Id } };
				var request = FormStandardRequest("mail/to/{farmerId}", urlSegments, Method.GET);
                var response = _restClient.Execute<List<Mail>>(request);

                var mail = new List<Mail>();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    mail.AddRange(response.Data);
                }

                return mail;
            });
        }

        private void AfterDayStarted(object sender, EventArgs e)
        {
            // Deliver mail each night
            SendItemsModEvents.RaiseOnMailDeliverySchedule(this, EventArgs.Empty);
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
                SendItemsModEvents.RaiseOnMailDeliverySchedule(this, EventArgs.Empty);
            }
        }

        private async Task UpdateLocalMail(List<Mail> mail)
        {
            await Task.Run(() =>
            {
                if (!mail.Any()) return;
                using (var db = new LiteRepository(_configService.ConnectionString))
                {
                    db.Update(mail);
                }
            });
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
