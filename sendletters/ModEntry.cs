using Autofac;
using Denifia.Stardew.SendLetters.Common.Domain;
using Denifia.Stardew.SendLetters.Domain;
using Denifia.Stardew.SendLetters.Services;
using StardewModdingAPI;
using System.Linq;

namespace Denifia.Stardew.SendLetters
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(helper).As<IModHelper>();
            builder.RegisterType<Repository>().As<IRepository>();
            builder.RegisterType<PlayerRepository>().As<IPlayerRepository>();
            builder.RegisterType<MessageRepository>().As<IMessageRepository>();
            builder.RegisterType<MemoryCache>().As<IMemoryCache>();
            
            builder.RegisterAssemblyTypes(typeof(ModEntry).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            
            var container = builder.Build();

            var program = new Program(
                container.Resolve<IConfigurationService>(),
                container.Resolve<IPlayerService>(),
                container.Resolve<IMessageService>(),
                container.Resolve<IMailboxService>()
            );

            program.Init();
        }
    }
}
