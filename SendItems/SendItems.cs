using System.Linq;
using Denifia.Stardew.SendItems.Services;
using Autofac;
using Denifia.Stardew.SendItems.Domain;
using StardewModdingAPI;
using System;
using StardewModdingAPI.Events;

namespace Denifia.Stardew.SendItems
{
    public class SendItems : Mod
    {
        private IContainer _container;
        private IFarmerService _farmerService;
        private IPostboxInteractionDetector _postboxInteractionDetector;
        private ILetterboxInteractionDetector _letterboxInteractionDetector;
        private IMailDeliveryService _mailDeliveryService;
        private IMailCleanupService _mailCleanupService;

        public override void Entry(IModHelper helper)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(this).As<IMod>();
            builder.RegisterInstance(helper).As<IModHelper>();
            builder.Register(c => new VersionCheckService(c.Resolve<IMod>()));
            builder.RegisterAssemblyTypes(typeof(SendItems).Assembly)
                .Where(t => t.Name.EndsWith("Service")) 
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(SendItems).Assembly)
                .Where(t => t.Name.EndsWith("Detector"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            _container = builder.Build();

            // Init repo first!
            Repository.Instance.Init(_container.Resolve<IConfigurationService>());

            // Instance classes that do their own thing
            _container.Resolve<VersionCheckService>();
            _container.Resolve<ICommandService>();
            _container.Resolve<IPostboxService>();
            _container.Resolve<ILetterboxService>();

            // Instance classes to be used later
            _farmerService = _container.Resolve<IFarmerService>();
            _postboxInteractionDetector = _container.Resolve<IPostboxInteractionDetector>();
            _letterboxInteractionDetector = _container.Resolve<ILetterboxInteractionDetector>();
            _mailDeliveryService = _container.Resolve<IMailDeliveryService>();
            _mailCleanupService = _container.Resolve<IMailCleanupService>();

            SaveEvents.AfterLoad += AfterSavedGameLoad;
        }

        private void AfterSavedGameLoad(object sender, EventArgs e)
        {
//            _farmerService.LoadCurrentFarmer();
            _postboxInteractionDetector.Init();
            _letterboxInteractionDetector.Init();
            _mailDeliveryService.Init();

            SaveEvents.AfterLoad -= AfterSavedGameLoad;
        }
    }
}
