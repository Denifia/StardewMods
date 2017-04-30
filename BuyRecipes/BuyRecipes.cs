using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyRecipes
{
    public class BuyRecipes : Mod
    {
        private readonly IMod _mod;

        public BuyRecipes()
        {
            _mod = this;
        }

        public override void Entry(IModHelper helper)
        {
            _mod.Monitor.Log("Loaded", LogLevel.Alert);
        }
    }
}
