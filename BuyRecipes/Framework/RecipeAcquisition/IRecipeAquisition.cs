namespace Denifia.Stardew.BuyRecipes.Framework.RecipeAcquisition
{
    public interface IRecipeAcquisition
    {
        bool AcceptsConditions(string condition);
        int Cost { get; }
    }
}
