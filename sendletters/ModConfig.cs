using System;

namespace Denifia.Stardew.SendLetters
{
    public class ModConfig
    {
        public Uri ApiUrl { get; set; }
        public bool Debug { get; set; }
        public bool CheckForUpdates { get; set; } = true;
    }
}