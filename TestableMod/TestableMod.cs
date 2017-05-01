using StardewModdingApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.TestableMod
{
    public class TestableMod
    {
        private readonly IMonitor _monitor;

        public TestableMod(IMonitor monitor)
        {
            _monitor = monitor;
            LogSomething();
        }

        public void LogSomething()
        {
            _monitor.Log("Hello World", LogLevel.Alert);
        }
    }
}
