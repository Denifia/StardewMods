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
        public override void Entry(IModHelper helper)
        {
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
    }
}
