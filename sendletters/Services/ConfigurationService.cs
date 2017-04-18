using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private IModHelper _modHelper;
        private ModConfig _modConfig;
        public string CurrentPlayerId { get; set; }

        public ConfigurationService(IModHelper modHelper)
        {
            _modHelper = modHelper;
            _modConfig = _modHelper.ReadConfig<ModConfig>();
        }

        public Uri GetApiUri()
        {
            return _modConfig.ApiUrl;
        }

        public string GetLocalPath()
        {
            return _modHelper.DirectoryPath;
        }

        public bool InDebugMode()
        {
            return _modConfig.Debug;
        }

        public bool InLocalOnlyMode()
        {
            return _modConfig.LocalOnly;
        }
    }
}
