namespace Denifia.Stardew.BuyRecipes.Framework.RecipePricing
{
    public class LevelBasedRecipePricing : BaseRecipePricing
    {
        private static readonly int pricePerLevel = 75;
        private static readonly int priceAtLevel100 = 1750;

        public static new bool TryCalculatePrice(string data, out int cost)
        {
            cost = -1;
            if (string.IsNullOrEmpty(data) || !data.StartsWith("l ")) return false;

            var dataParts = data.Split(' ');
            var playerLevel = int.Parse(dataParts[1]);

            if (playerLevel == 100)
            {
                cost = priceAtLevel100;
            }
            else
            {
                cost = playerLevel * pricePerLevel;
            }
            
            return true;
        }
    }
}
