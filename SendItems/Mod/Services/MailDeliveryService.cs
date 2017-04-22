using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Denifia.Stardew.SendItems.Domain;
using LiteDB;
using RestSharp;

namespace Denifia.Stardew.SendItems.Services
{
    public interface IMailDeliveryService
    {
        Task DeliverPostedMail();
    }

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

            // Hook up events
        }

        public async Task DeliverPostedMail()
        {
            await DeliverLocalMail();
            if (_configService.InLocalOnlyMode())
            {
                await DeliverCloudMail();    
            }
        }

        private async Task DeliverLocalMail()
        {
            var localMail = await GetLocallyPostedMail();
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

            foreach (var mail in updatedLocalMail)
            {
				// TODO: Update  mail in local DB
			}
        }

        private async Task DeliverCloudMail()
        {
            var RemoteMail = await GetRemotelyPostedMail();

            // TODO: Complete method
        }

        private async Task<List<Mail>> GetLocallyPostedMail()
        {
            return await Task.Run(() =>
            {
                using (var db = new LiteRepository(_configService.GetLocalPath()))
                {
                    return db.Query<Mail>().Where(x => x.Status == MailStatus.Posted).ToList();
                }
            });
        }

        private async Task<List<Mail>> GetRemotelyPostedMail()
        {
            return await Task.Run(() =>
            {
                var urlSegments = new Dictionary<string, string> { { "farmerId", _farmerService.CurrentFarmer.Id } };
				var request = FormStandardRequest("mail/to/{farmerId}", urlSegments, Method.GET);
                var respone = _restClient.Execute<List<Mail>>(request);

                var mail = new List<Mail>();
                if (respone.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    mail.AddRange(respone.Data);
                }

                return mail;
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
