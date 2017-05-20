namespace Denifia.Stardew.BuyRecipes.Framework.RecipePricing
{
    public class BaseRecipePricing
    {
        private static readonly int defaultPrice = 1000;

        public static bool TryCalculatePrice(string data, out int price)
        {
            price = defaultPrice;
            return true;
        }
    }
}
