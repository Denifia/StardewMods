using denifia.stardew.sendletters.Domain;
using denifia.stardew.sendletters.Models;
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
            var program = new Program(config);           
        }
    }
}
