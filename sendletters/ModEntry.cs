using Autofac;
using Denifia.Stardew.SendLetters.Common.Domain;
using Denifia.Stardew.SendLetters.Domain;
using Denifia.Stardew.SendLetters.Services;
using Pathoschild.Stardew.Common;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters
{
    public class ModEntry : Mod
    {
        private ModConfig Config;
        private ISemanticVersion CurrentVersion;
        private ISemanticVersion NewRelease;
        private bool HasSeenUpdateWarning;

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
            builder.RegisterType<Repository>().As<IRepository>();
            builder.RegisterType<PlayerRepository>().As<IPlayerRepository>();
            builder.RegisterType<MessageRepository>().As<IMessageRepository>();
            builder.RegisterType<MemoryCache>().As<IMemoryCache>();
            
            builder.RegisterAssemblyTypes(typeof(SendLetterMod).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            
            var container = builder.Build();

            var program = new SendLetterMod(this, helper,
                container.Resolve<IConfigurationService>(),
                container.Resolve<IPlayerService>(),
                container.Resolve<IMessageService>(),
                container.Resolve<IMailboxService>()
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
                            ISemanticVersion latest = UpdateHelper.LogVersionCheck(this.Monitor, this.ModManifest.Version, "SendLetters").Result;
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
                    CommonHelper.ShowInfoMessage($"You can update Automate from {this.CurrentVersion} to {this.NewRelease}.");
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
