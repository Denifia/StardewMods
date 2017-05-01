using Autofac;
using Denifia.Stardew.TestableMod;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestableModWrapper
{
    public class ModEntry : Mod
    { 
        private IContainer _container;

        public override void Entry(IModHelper helper)
        {
            Startup();

            _container.Resolve<TestableMod>();
        }

        private void Startup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(Monitor).As<IMonitor>();
            builder.Register(c => new Denifia.Stardew.TestableMod.TestableMod(c.Resolve<IMonitor>()));
            _container = builder.Build();
        }
    }
}
