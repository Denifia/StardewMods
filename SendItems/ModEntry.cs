using Autofac;
using StardewModdingAPI;
using System;
using System.Linq;
using System.Threading.Tasks;
using Denifia.Stardew.SendItems.Services;
using Denifia.Stardew.SendItems.Framework;

namespace Denifia.Stardew.SendItems
{
    public class ModEntry : Mod
    {
        private ModConfig Config;
        private ISemanticVersion CurrentVersion;
        private ISemanticVersion NewRelease;
        private bool HasSeenUpdateWarning;

        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();
            CurrentVersion = ModManifest.Version;

            // TODO: Uncomment
            //GameEvents.GameLoaded += GameEvents_GameLoaded;
            //SaveEvents.AfterLoad += SaveEvents_AfterLoad;

            // TODO: Add another scan for "Detector"
            var builder = new ContainerBuilder();
            builder.RegisterInstance(this).As<IMod>();
            builder.RegisterInstance(helper).As<IModHelper>();
            builder.RegisterAssemblyTypes(typeof(SendItems).Assembly)
                .Where(t => t.Name.EndsWith("Service")) 
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            
            var container = builder.Build();

            var program = new SendItems(this,
                container.Resolve<IConfigurationService>(),
                container.Resolve<ICommandService>(),
                container.Resolve<IFarmerService>(),
                container.Resolve<IPostboxService>(),
                container.Resolve<IPostboxInteractionService>(),
                container.Resolve<ILetterboxService>(),
                container.Resolve<ILetterboxInteractionService>(),
                container.Resolve<IMailDeliveryService>()
            );
        }

        // TODO: Move the below methods into a VersionCheckService

        private void GameEvents_GameLoaded(object sender, EventArgs e)
        {
            // check for mod update
            if (Config.CheckForUpdates)
            {
                try
                {
                    Task.Factory.StartNew(() =>
                    {
                        ISemanticVersion latest = UpdateHelper.LogVersionCheck(Monitor, ModManifest.Version).Result;
                        if (latest.IsNewerThan(CurrentVersion))
                            NewRelease = latest;
                    });
                }
                catch (Exception ex)
                {
                    HandleError(ex, "checking for a new version");
                }
            }
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            // render update warning
            if (Config.CheckForUpdates && !HasSeenUpdateWarning && NewRelease != null)
            {
                try
                {
                    HasSeenUpdateWarning = true;
                    ModHelper.ShowInfoMessage($"You can update {ModConstants.ModName} from {CurrentVersion} to {NewRelease}.");
                }
                catch (Exception ex)
                {
                    HandleError(ex, "showing the new version available");
                }
            }
        }

        private void HandleError(Exception ex, string verb)
        {
            Monitor.Log($"Something went wrong {verb}:\n{ex}", LogLevel.Error);
            ModHelper.ShowErrorMessage($"Huh. Something went wrong {verb}. The error log has the technical details.");
        }
    }
}
