using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.BuyRecipes
{
    public class BuyRecipes : Mod
    {
        private List<CookingRecipe> CookingRecipes;

        public BuyRecipes()
        {

        }

        public override void Entry(IModHelper helper)
        {
            CookingRecipes = new List<CookingRecipe>();
            foreach (var recipe in CraftingRecipe.cookingRecipes)
            {
                CookingRecipes.Add(new CookingRecipe(recipe.Key, recipe.Value));
            }
        }
    }

    public class CookingRecipe
    {
        public string Name { get; set; }
        public List<ItemWithQuantity> Ingredients { get; set; }
        public ItemWithQuantity ResultingItem { get; set; }

        public CookingRecipe(string name, string data)
        {
            Name = name;

            var dataParts = data.Split('/');

            var ingredientsData = dataParts[0];
            Ingredients = DeserializeIngredients(ingredientsData);

            var unknownData = dataParts[1];

            var resultingItemData = dataParts[2];
            ResultingItem = DeserializeResultingItem(resultingItemData);

            var aquisitionData = dataParts[3];
        }

        private List<ItemWithQuantity> DeserializeIngredients(string data)
        {
            var ingredients = new List<ItemWithQuantity>();
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

        private ItemWithQuantity DeserializeResultingItem(string data)
        {
            var dataParts = data.Split(' ');
            if (dataParts.Count() == 1)
            {
                // Default amount of an item is 1
                return DeserializeItemWithQuantity(dataParts[0], "1");
            }
            return DeserializeItemWithQuantity(dataParts[0], dataParts[1]);
        }

        private ItemWithQuantity DeserializeItemWithQuantity(string itemId, string quantity)
        {
            var itemWithQuantity = new ItemWithQuantity
            {
                Id = int.Parse(itemId),
                Quantity = int.Parse(quantity),
            };

            var objectData = string.Empty;
            Game1.objectInformation.TryGetValue(itemWithQuantity.Id, out objectData);
            if (objectData != string.Empty)
            {
                var name = objectData.Split('/')[4];
                itemWithQuantity.Name = name;
            }

            return itemWithQuantity;
        }
    }

    public class ItemWithQuantity
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
