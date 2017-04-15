using denifia.stardew.sendletters.Domain;
using System;
using System.Collections.Generic;

namespace denifia.stardew.sendletters
{
    public class ModConfig
    {
        public Uri ApiUrl { get; set; }
        public List<Player> Players { get; set; }
        public bool Debug { get; set; }
    }
}