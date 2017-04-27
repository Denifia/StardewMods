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
        void Init();
    }

    public class MailCleanupService : IMailCleanupService
    {
        private readonly IMod _mod;
        private readonly IConfigurationService _configService;
        private readonly IFarmerService _farmerService;
        private RestClient _restClient { get; set; }

        // TODO: Flesh out MailCleanupService

        public MailCleanupService(IMod mod, IConfigurationService configService, IFarmerService farmerService)
        {
            _mod = mod;
            _configService = configService;
            _farmerService = farmerService;
            _restClient = new RestClient(_configService.GetApiUri());
        }

        public void Init()
        {

        }

        // DeleteReadMail
    }
}
