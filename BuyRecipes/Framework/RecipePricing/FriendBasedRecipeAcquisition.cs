namespace Denifia.Stardew.BuyRecipes.Framework.RecipePricing
{
    public class FriendBasedRecipePricing : BaseRecipePricing
    {
        private static readonly int pricePerLevel = 600;

        public static new bool TryCalculatePrice(string data, out int price)
        {
            price = -1;
            if (string.IsNullOrEmpty(data) || !data.StartsWith("f ")) return false;

            var dataParts = data.Split(' ');
            var friend = dataParts[1];
            var friendLevel = int.Parse(dataParts[2]);

            price = friendLevel * pricePerLevel;
            return true;
        }
    }
}
