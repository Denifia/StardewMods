using Autofac;
using Pathoschild.Stardew.Common;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using System.Linq;
using System.Threading.Tasks;
using Denifia.Stardew.SendItems.Services;

namespace Denifia.Stardew.SendItems
{
    public class ModEntry : Mod
    {
        private ModConfig Config;
        private ISemanticVersion CurrentVersion;
        private ISemanticVersion NewRelease;
        private bool HasSeenUpdateWarning;

        private string ModName = "SendItems";

        public override void Entry(IModHelper helper)
        {
            // read config
            Config = helper.ReadConfig<ModConfig>();
            CurrentVersion = ModManifest.Version;

            // hooks for update check
            GameEvents.GameLoaded += GameEvents_GameLoaded;
            SaveEvents.AfterLoad += SaveEvents_AfterLoad;

            var builder = new ContainerBuilder();
            builder.RegisterInstance(helper).As<IModHelper>();
            builder.RegisterAssemblyTypes(typeof(SendItems).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            
            var container = builder.Build();

            var program = new SendItems(this,
                container.Resolve<IConfigurationService>(),
                container.Resolve<IFarmerService>(),
                container.Resolve<IPostboxService>()
            );
        }

        /****
        ** Version Checking
        ****/

        private void GameEvents_GameLoaded(object sender, EventArgs e)
        {
            // check for mod update
            if (this.Config.CheckForUpdates)
            {
                try
                {
                    Task.Factory.StartNew(() =>
                    {
                        Task.Factory.StartNew(() =>
                        {
                            ISemanticVersion latest = UpdateHelper.LogVersionCheck(this.Monitor, this.ModManifest.Version, ModName).Result;
                            if (latest.IsNewerThan(this.CurrentVersion))
                                this.NewRelease = latest;
                        });
                    });
                }
                catch (Exception ex)
                {
                    this.HandleError(ex, "checking for a new version");
                }
            }
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            // render update warning
            if (this.Config.CheckForUpdates && !this.HasSeenUpdateWarning && this.NewRelease != null)
            {
                try
                {
                    this.HasSeenUpdateWarning = true;
                    CommonHelper.ShowInfoMessage($"You can update {ModName} from {this.CurrentVersion} to {this.NewRelease}.");
                }
                catch (Exception ex)
                {
                    this.HandleError(ex, "showing the new version available");
                }
            }
        }

        private void HandleError(Exception ex, string verb)
        {
            this.Monitor.Log($"Something went wrong {verb}:\n{ex}", LogLevel.Error);
            CommonHelper.ShowErrorMessage($"Huh. Something went wrong {verb}. The error log has the technical details.");
        }
    }
}
