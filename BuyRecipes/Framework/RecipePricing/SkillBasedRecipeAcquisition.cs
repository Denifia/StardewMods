namespace Denifia.Stardew.BuyRecipes.Framework.RecipePricing
{
    public class SkillBasedRecipePricing : BaseRecipePricing
    {
        private static readonly int pricePerLevel = 900;

        public static new bool TryCalculatePrice(string data, out int cost)
        {
            cost = -1;
            if (string.IsNullOrEmpty(data) || !data.StartsWith("s ")) return false;

            var dataParts = data.Split(' ');
            var skill = dataParts[1];
            var skillLevel = int.Parse(dataParts[2]);

            cost = skillLevel * pricePerLevel;
            return true;
        }
    }
}
