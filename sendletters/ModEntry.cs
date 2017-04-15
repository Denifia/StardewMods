using Autofac;
using denifia.stardew.sendletters.Domain;
using denifia.stardew.sendletters.Models;
using denifia.stardew.sendletters.Services;
using RestSharp;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace denifia.stardew.sendletters
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            var config = helper.ReadConfig<ModConfig>();

            var builder = new ContainerBuilder();
            builder.RegisterInstance(helper).As<IModHelper>();
            // ToDo: switch repos based on config
            builder.RegisterType<LocalRepository>().As<IRepository>();
            builder.RegisterAssemblyTypes(typeof(ModEntry).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            //builder.RegisterType<ConfigurationService>().As<IConfigurationService>();
            var container = builder.Build();

            var program = new Program(helper, 
                container.Resolve<IRepository>(), 
                container.Resolve<IConfigurationService>(),
                container.Resolve<IPlayerService>());

            program.Init();
        }
    }
}
