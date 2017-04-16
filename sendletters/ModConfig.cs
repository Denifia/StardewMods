using denifia.stardew.sendletters.Domain;
using System;
using System.Collections.Generic;

namespace denifia.stardew.sendletters
{
    public class ModConfig
    {
        public Uri ApiUrl { get; set; }
        public bool Debug { get; set; }
        public bool LocalOnly { get; set; }
    }
}