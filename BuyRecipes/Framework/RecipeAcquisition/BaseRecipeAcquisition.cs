namespace Denifia.Stardew.BuyRecipes.Framework.RecipeAcquisition
{
    public class BaseRecipeAcquisition : IRecipeAcquisition
    {
        public virtual int Cost => 1000;

        public BaseRecipeAcquisition() { }

        public BaseRecipeAcquisition(string data) { }

        public virtual bool AcceptsConditions(string condition) => true;
    }
}
