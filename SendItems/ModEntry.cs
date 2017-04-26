using System.Linq;
using Denifia.Stardew.SendItems.Services;
using StardewModdingAPI;
using Autofac;

namespace Denifia.Stardew.SendItems
{
    public class ModEntry : Mod
    {
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
            var container = builder.Build();

            // Instance classes that do their own thing
            container.Resolve<VersionCheckService>();

            var program = new SendItems(this,
                container.Resolve<IConfigurationService>(),
                container.Resolve<ICommandService>(),
                container.Resolve<IFarmerService>(),
                container.Resolve<IPostboxService>(),
                container.Resolve<IPostboxInteractionDetector>(),
                container.Resolve<ILetterboxService>(),
                container.Resolve<ILetterboxInteractionDetector>(),
                container.Resolve<IMailDeliveryService>()
            );
        }
    }
}
