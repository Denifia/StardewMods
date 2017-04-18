using Autofac;
using Denifia.Stardew.SendLetters.Domain;
using Denifia.Stardew.SendLetters.Models;
using Denifia.Stardew.SendLetters.Services;
using RestSharp;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Denifia.Stardew.SendLetters
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            var config = helper.ReadConfig<ModConfig>();

            var builder = new ContainerBuilder();
            builder.RegisterInstance(helper).As<IModHelper>();
            // ToDo: switch repos based on config
            if (config.LocalOnly)
            {
                builder.RegisterType<LocalRepository>().As<OldIRepository>();
            }
            else
            {
                builder.RegisterType<LocalAndRemoteRepository>().As<OldIRepository>();
            }
            
            builder.RegisterAssemblyTypes(typeof(ModEntry).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            //builder.RegisterType<ConfigurationService>().As<IConfigurationService>();
            var container = builder.Build();

            var program = new Program(helper, 
                container.Resolve<OldIRepository>(), 
                container.Resolve<IConfigurationService>(),
                container.Resolve<IPlayerService>(),
                container.Resolve<IMessageService>(),
                container.Resolve<IMailboxService>());

            program.Init();
        }
    }
}
