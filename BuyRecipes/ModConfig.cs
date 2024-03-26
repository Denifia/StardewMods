namespace Denifia.Stardew.BuyRecipes
{
    public sealed class ModConfig
    {
        public int maxNumberOfRecipesPerWeek { get; set; }

        public ModConfig()
        {
            this.maxNumberOfRecipesPerWeek = 5;
        }
    }
}