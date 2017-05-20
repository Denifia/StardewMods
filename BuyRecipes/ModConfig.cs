using System;

namespace Denifia.Stardew.BuyRecipes
{
    public class ModConfig
    {
        public bool Debug { get; set; }
        public bool CheckForUpdates { get; set; } = true;
        public int MaxNumberOfRecipesPerWeek { get; set; } = 5;
    }
}