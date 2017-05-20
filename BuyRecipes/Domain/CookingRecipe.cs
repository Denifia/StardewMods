using Denifia.Stardew.BuyRecipes.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace Denifia.Stardew.BuyRecipes.Domain
{
    internal class CookingRecipe
    {
        private string _name;
        private IEnumerable<GameItemWithQuantity> _ingredients;
        private GameItemWithQuantity _resultingItem;
        private int _cost;

        public string Name => _name;
        public IEnumerable<GameItemWithQuantity> Ingredients => _ingredients;
        public GameItemWithQuantity ResultingItem => _resultingItem;
        public int Cost => _cost;

        public static CookingRecipe Deserialise(string name, string data)
        {
            var cookingRecipeData = CookingRecipeData.Deserialise(data);
            var ingredients = IngredientFactory.DeserializeIngredients(cookingRecipeData.IngredientsData);
            var resultingItem = IngredientFactory.DeserializeIngredient(cookingRecipeData.ResultingItemData);
            var cost = RecipePricingFactory.CalculatePrice(cookingRecipeData.AcquisitionData);
            return new CookingRecipe(name, ingredients, resultingItem, cost);
        }

        public CookingRecipe(string name, 
            IEnumerable<GameItemWithQuantity> ingredients, 
            GameItemWithQuantity resultingItem,
            int cost)
        {
            _name = name;
            _ingredients = ingredients;
            _resultingItem = resultingItem;
            _cost = cost;
        }
    }

    
}
