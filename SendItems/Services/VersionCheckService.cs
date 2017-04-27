using Denifia.Stardew.SendItems.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendItems.Services
{
    public class VersionCheckService
    {
        private IMod _mod;
        private ModConfig _config;
        private ISemanticVersion _currentVersion;
        private ISemanticVersion _newRelease;
        private bool _hasSeenUpdateWarning;

        public VersionCheckService(IMod mod)
        {
            _mod = mod;
            _config = mod.Helper.ReadConfig<ModConfig>();
            _currentVersion = mod.ModManifest.Version;

            GameEvents.GameLoaded += GameEvents_GameLoaded;
            SaveEvents.AfterLoad += SaveEvents_AfterLoad;
        }

        private void GameEvents_GameLoaded(object sender, EventArgs e)
        {
            // check for mod update
            if (_config.CheckForUpdates)
            {
                try
                {
                    Task.Factory.StartNew(() =>
                    {
                        ISemanticVersion latest = UpdateHelper.LogVersionCheck(_mod.Monitor, _mod.ModManifest.Version).Result;
                        if (latest.IsNewerThan(_currentVersion))
                            _newRelease = latest;
                    });
                }
                catch (Exception ex)
                {
                    ModHelper.HandleError(_mod, ex, "checking for a new version");
                }
            }
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            // render update warning
            if (_config.CheckForUpdates && !_hasSeenUpdateWarning && _newRelease != null)
            {
                try
                {
                    _hasSeenUpdateWarning = true;
                    ModHelper.ShowInfoMessage($"You can update {ModConstants.ModName} from {_currentVersion} to {_newRelease}.");
                }
                catch (Exception ex)
                {
                    ModHelper.HandleError(_mod, ex, "showing the new version available");
                }
            }
        }
    }
}
