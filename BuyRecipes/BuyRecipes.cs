using StardewModdingAPI;
using StardewModdingAPI.Events;
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
        private bool _savedGameLoaded = false;
        private List<CookingRecipe> _cookingRecipes;
        private static List<GameItem> _gameObjects;
        public static List<GameItem> GameObjects
        {
            get
            {
                if (_gameObjects == null)
                {
                    DeserializeGameObjects();
                }
                return _gameObjects;
            }
        }
        public static List<IRecipeAquisitionConditions> RecipeAquisitionConditions;

        public override void Entry(IModHelper helper)
        {
            SaveEvents.AfterLoad += SaveEvents_AfterLoad;
            SaveEvents.AfterReturnToTitle += SaveEvents_AfterReturnToTitle;

            RecipeAquisitionConditions = new List<IRecipeAquisitionConditions>()
            {
                new FriendBasedRecipeAquisition(),
                new SkillBasedRecipeAquisition(),
                new LevelBasedRecipeAquisition()
            };

            helper.ConsoleCommands.Add("buy", $"temp", HandleCommand);
        }

        private void HandleCommand(string command, string[] args)
        {
            if (!_savedGameLoaded)
            {
                Monitor.Log("Please load up a saved game first, then try again.", LogLevel.Warn);
                return;
            }

            switch (command)
            {
                case "buy":
                    BuyRecipe(args);
                    break;
                default:
                    throw new NotImplementedException($"Send Items received unknown command '{command}'.");
            }
        }

        private void LogUsageError(string error, string command)
        {
            Monitor.Log($"{error} Type 'help {command}' for usage.", LogLevel.Error);
        }

        private void LogArgumentsInvalid(string command)
        {
            LogUsageError("The arguments are invalid.", command);
        }

        private void BuyRecipe(string[] args)
        {
            
            if (args.Length == 1)
            {
                var recipeName = args[0];
                var recipe = _cookingRecipes.FirstOrDefault(x => x.Name == recipeName);
                if (recipe == null)
                {
                    Monitor.Log("Recipe not found", LogLevel.Info);
                    return;
                }

                if (recipe.IsKnown || Game1.player.cookingRecipes.ContainsKey(recipeName))
                {
                    recipe.IsKnown = true;
                    Monitor.Log("Recipe already known", LogLevel.Info);
                    return;
                }

                Game1.player.cookingRecipes.Add(recipeName, 0);
                Game1.player.Money -= recipe.AquisitionConditions.Cost;
                Monitor.Log($"{recipeName} bought for ${recipe.AquisitionConditions.Cost}!", LogLevel.Alert);
            }
            else
            {
                LogArgumentsInvalid("buy");
            }
        }

        private void SaveEvents_AfterReturnToTitle(object sender, EventArgs e)
        {
            _savedGameLoaded = false;
            ResetRecipes();
        }

        private void ResetRecipes()
        {
            _cookingRecipes = null;
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            _savedGameLoaded = true;
            DiscoverRecipes();

            foreach (var item in _cookingRecipes.Where(x => !x.IsKnown).OrderBy(x => x.AquisitionConditions.Cost))
            {
                this.Monitor.Log($"{item.AquisitionConditions.Cost} - {item.Name}", LogLevel.Info);
            }
        }

        private void DiscoverRecipes()
        {
            var knownRecipes = Game1.player.cookingRecipes.Keys;
            _cookingRecipes = new List<CookingRecipe>();
            foreach (var recipe in CraftingRecipe.cookingRecipes)
            {
                var cookingRecipe = new CookingRecipe(recipe.Key, recipe.Value);
                if (Game1.player.cookingRecipes.ContainsKey(cookingRecipe.Name))
                {
                    cookingRecipe.IsKnown = true;
                }
                _cookingRecipes.Add(cookingRecipe);
            }

            var unknownRecipeCount = _cookingRecipes.Where(x => !x.IsKnown).Count();
        }

        private static void DeserializeGameObjects()
        {
            _gameObjects = new List<GameItem>();
            foreach (var item in Game1.objectInformation)
            {
                _gameObjects.Add(new GameItem
                {
                    Id = item.Key,
                    Name = item.Value.Split('/')[4]
                });
            }
        }
    }

    public class CookingRecipe
    {
        public string Name { get; set; }
        public List<GameItemWithQuantity> Ingredients { get; set; }
        public GameItemWithQuantity ResultingItem { get; set; }
        public IRecipeAquisitionConditions AquisitionConditions { get; set; }
        public bool IsKnown { get; set; }

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
            var aquisitionConditions = BuyRecipes.RecipeAquisitionConditions.FirstOrDefault(x => x.AcceptsConditions(aquisitionData));
            if (aquisitionConditions == null)
            {
                AquisitionConditions = new DefaultRecipeAquisition(aquisitionData);
            }
            else
            {
                AquisitionConditions = (IRecipeAquisitionConditions)Activator.CreateInstance(aquisitionConditions.GetType(), new object[] { aquisitionData });
            }
        }

        private List<GameItemWithQuantity> DeserializeIngredients(string data)
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

        private GameItemWithQuantity DeserializeResultingItem(string data)
        {
            var dataParts = data.Split(' ');
            if (dataParts.Count() == 1)
            {
                // Default amount of an item is 1
                return DeserializeItemWithQuantity(dataParts[0], "1");
            }
            return DeserializeItemWithQuantity(dataParts[0], dataParts[1]);
        }

        private GameItemWithQuantity DeserializeItemWithQuantity(string itemId, string quantity)
        {
            var itemWithQuantity = new GameItemWithQuantity
            {
                Id = int.Parse(itemId),
                Quantity = int.Parse(quantity),
            };

            var gameItem = BuyRecipes.GameObjects.FirstOrDefault(x => x.Id == itemWithQuantity.Id);
            if (gameItem != null)
            {
                itemWithQuantity.Name = gameItem.Name;
            }

            return itemWithQuantity;
        }
    }

    public class GameItem
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class GameItemWithQuantity : GameItem
    {
        public int Quantity { get; set; }
    }

    public interface IRecipeAquisitionConditions
    {
        bool AcceptsConditions(string condition);
        int Cost { get; }
    }

    public abstract class BaseRecipeAquisition
    {
        public BaseRecipeAquisition()
        {
        }

        public BaseRecipeAquisition(string data)
        {
        }
    }

    public class FriendBasedRecipeAquisition : BaseRecipeAquisition, IRecipeAquisitionConditions
    {
        private int _friendLevel;
        private string _friend;
        public int Cost => _friendLevel * 600;

        public FriendBasedRecipeAquisition() { }

        public FriendBasedRecipeAquisition(string data) : base(data)
        {
            var dataParts = data.Split(' ');
            _friend = dataParts[1];
            _friendLevel = int.Parse(dataParts[2]);
        }

        public bool AcceptsConditions(string condition)
        {
            return condition.StartsWith("f ");
        }
    }

    public class SkillBasedRecipeAquisition : BaseRecipeAquisition, IRecipeAquisitionConditions
    {
        private int _skillLevel;
        private string _skill;
        public int Cost => _skillLevel * 900;

        public SkillBasedRecipeAquisition() { }

        public SkillBasedRecipeAquisition(string data) : base(data)
        {
            var dataParts = data.Split(' ');
            _skill = dataParts[1];
            _skillLevel = int.Parse(dataParts[2]);
        }

        public bool AcceptsConditions(string condition)
        {
            return condition.StartsWith("s ");
        }
    }

    public class LevelBasedRecipeAquisition : BaseRecipeAquisition, IRecipeAquisitionConditions
    {
        private int _playerLevel;
        public int Cost => _playerLevel * 900;

        public LevelBasedRecipeAquisition() { }

        public LevelBasedRecipeAquisition(string data) : base(data)
        {
            var dataParts = data.Split(' ');
            _playerLevel = int.Parse(dataParts[1]);
        }

        public bool AcceptsConditions(string condition)
        {
            return condition.StartsWith("l ");
        }

        private int GetCost()
        {
            if (_playerLevel == 100) return 1750;
            return _playerLevel * 75;
        }
    }

    public class DefaultRecipeAquisition : BaseRecipeAquisition, IRecipeAquisitionConditions
    {
        public int Cost => 1000;

        public DefaultRecipeAquisition() { }

        public DefaultRecipeAquisition(string data) : base(data) { }

        public bool AcceptsConditions(string condition)
        {
            return true;
        }
    }
}
