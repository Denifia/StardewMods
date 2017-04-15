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

        public ConfigurationService(IModHelper modHelper)
        {
            _modHelper = modHelper;
        }

        public string GetLocalPath()
        {
            return _modHelper.DirectoryPath;
        }
    }
}
