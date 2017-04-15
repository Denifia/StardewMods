using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private IModHelper _modHelper;
        private ModConfig _modConfig;

        public ConfigurationService(IModHelper modHelper)
        {
            _modHelper = modHelper;
            _modConfig = _modHelper.ReadConfig<ModConfig>();
        }

        public string GetLocalPath()
        {
            return _modHelper.DirectoryPath;
        }

        public bool InDebugMode()
        {
            return _modConfig.Debug;
        }

    }
}
