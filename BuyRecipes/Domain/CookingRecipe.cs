using Denifia.Stardew.BuyRecipes.Framework;
using Denifia.Stardew.BuyRecipes.Framework.RecipeAcquisition;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Denifia.Stardew.BuyRecipes.Domain
{
    public class CookingRecipe
    {
        private string _name;
        public string Name => _name;

        private List<GameItemWithQuantity> _ingredients;
        public List<GameItemWithQuantity> Ingredients => _ingredients;

        private GameItemWithQuantity _resultingItem;
        public GameItemWithQuantity ResultingItem => _resultingItem;

        private IRecipeAcquisition _acquisitionConditions;
        public IRecipeAcquisition AcquisitionConditions => _acquisitionConditions;

        public static CookingRecipe Create(string name, string data)
        {
            var dataParts = data.Split('/');
            var ingredientsData = dataParts[0];
            var unknownData = dataParts[1];
            var resultingItemData = dataParts[2];
            var acquisitionData = dataParts[3];

            return new CookingRecipe
            {
                _name = name,
                _ingredients = DeserializeIngredients(ingredientsData),
                _resultingItem = DeserializeResultingItem(resultingItemData),
                _acquisitionConditions = AquisitionFactory.Instance.GetAquisitionImplementation(acquisitionData)
            };
        }

        private CookingRecipe() { }

        private static List<GameItemWithQuantity> DeserializeIngredients(string data)
        {
            var ingredients = new List<GameItemWithQuantity>();
            var dataParts = data.Split(' ');
            for (int i = 0; i < dataParts.Count(); i++)
            {
                try
                {
                    var ingredientData = DeserializeItemWithQuantity(dataParts[i], dataParts[i + 1]);
                    ingredients.Add(ingredientData);

                    i++; // Skip in pairs
                }
                catch (Exception ex)
                {
                }
            }
            return ingredients;
        }

        private static GameItemWithQuantity DeserializeResultingItem(string data)
        {
            var dataParts = data.Split(' ');
            if (dataParts.Count() == 1)
            {
                // Default amount of an item is 1
                return DeserializeItemWithQuantity(dataParts[0], "1");
            }
            return DeserializeItemWithQuantity(dataParts[0], dataParts[1]);
        }

        private static GameItemWithQuantity DeserializeItemWithQuantity(string itemId, string quantity)
        {
            var itemWithQuantity = new GameItemWithQuantity
            {
                Id = int.Parse(itemId),
                Quantity = int.Parse(quantity),
            };

            var gameItem = ModHelper.GameObjects.FirstOrDefault(x => x.Id == itemWithQuantity.Id);
            if (gameItem != null)
            {
                itemWithQuantity.Name = gameItem.Name;
            }

            return itemWithQuantity;
        }
    }
}
