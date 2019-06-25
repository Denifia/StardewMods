﻿using System;
using System.Collections.Generic;
using System.Linq;
using Denifia.Stardew.BuyRecipes.Domain;
using Denifia.Stardew.BuyRecipes.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Denifia.Stardew.BuyRecipes
{
    /// <summary>The mod entry class.</summary>
    public class BuyRecipes : Mod
    {
        private bool _savedGameLoaded = false;
        private List<CookingRecipe> _cookingRecipes;
        private List<CookingRecipe> _thisWeeksRecipes;
        private int _seed;

        public static List<IRecipeAquisitionConditions> RecipeAquisitionConditions;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.ReturnedToTitle += OnReturnedToTitle;
            helper.Events.GameLoop.DayStarted += OnDayStarted;

            RecipeAquisitionConditions = new List<IRecipeAquisitionConditions>()
            {
                new FriendBasedRecipeAquisition(),
                new SkillBasedRecipeAquisition(),
                new LevelBasedRecipeAquisition()
            };

            helper.ConsoleCommands
                .Add("buyrecipe", "Buy a recipe. \n\nUsage: buyrecipe \"<name of recipe>\" \n\nNote: This is case sensitive!", HandleCommand)
                .Add("showrecipes", "Lists this weeks available recipes. \n\nUsage: showrecipes", HandleCommand);
                //.Add("buyallrecipes", $"Temporary. \n\nUsage: buyallrecipes", HandleCommand);
        }

        private void HandleCommand(string command, string[] args)
        {
            args = new List<string> { string.Join(" ", args) }.ToArray();

            if (!_savedGameLoaded)
            {
                Monitor.Log("Please load up a saved game first, then try again.", LogLevel.Warn);
                return;
            }

            switch (command)
            {
                case "buyrecipe":
                    BuyRecipe(args);
                    break;
                case "showrecipes":
                    ShowWeeklyRecipes();
                    break;
                case "buyallrecipes":
                    BuyAllRecipes();
                    break;
                default:
                    throw new NotImplementedException($"Send Items received unknown command '{command}'.");
            }
        }

        private void BuyAllRecipes()
        {
            foreach (var recipe in _cookingRecipes.Where(x => !x.IsKnown).ToList())
            {
                BuyRecipe(new string[] { recipe.Name }, false);
            }
        }

        private void BuyRecipe(string[] args, bool checkInWeeklyRecipes = true)
        {
            if (args.Length == 1)
            {
                var recipeName = args[0].Trim('"');
                var recipe = _cookingRecipes.FirstOrDefault(x => x.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
                if (recipe == null)
                {
                    Monitor.Log("Recipe not found", LogLevel.Info);
                    return;
                }

                // Use the explicit name
                recipeName = recipe.Name;

                if (recipe.IsKnown || Game1.player.cookingRecipes.ContainsKey(recipeName))
                {
                    recipe.IsKnown = true;
                    Monitor.Log("Recipe already known", LogLevel.Info);
                    return;
                }

                if (checkInWeeklyRecipes && !_thisWeeksRecipes.Any(x => x.Name.Equals(recipeName)))
                {
                    Monitor.Log("Recipe is not availble to buy this week", LogLevel.Info);
                    return;
                }

                if (Game1.player.Money < recipe.AquisitionConditions.Cost)
                {
                    Monitor.Log("You can't affort this recipe", LogLevel.Info);
                    return;
                }

                Game1.player.cookingRecipes.Add(recipeName, 0);
                Game1.player.Money -= recipe.AquisitionConditions.Cost;
                recipe.IsKnown = true;
                Monitor.Log($"{recipeName} bought for {ModHelper.GetMoneyAsString(recipe.AquisitionConditions.Cost)}!", LogLevel.Alert);
            }
            else
            {
                LogArgumentsInvalid("buy");
            }
        }

        /// <summary>Raised after the game returns to the title screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnReturnedToTitle(object sender, ReturnedToTitleEventArgs e)
        {
            _savedGameLoaded = false;
            _cookingRecipes = null;
            _thisWeeksRecipes = null;
        }

        /// <summary>Raised after the player loads a save slot and the world is initialised.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            _savedGameLoaded = true;
            DiscoverRecipes();
            GenerateWeeklyRecipes();
        }

        /// <summary>Raised after the game begins a new day (including when the player loads a save).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            GenerateWeeklyRecipes();
        }

        private void GenerateWeeklyRecipes()
        {
            var gameDateTime = new GameDateTime(Game1.timeOfDay, Game1.dayOfMonth, Game1.currentSeason, Game1.year);
            var startDayOfWeek = (((gameDateTime.DayOfMonth / 7) + 1) * 7) - 6;
            var seed = int.Parse($"{startDayOfWeek}{gameDateTime.Season}{gameDateTime.Year}");
            var random = new Random(seed);

            if (_seed == seed) return;
            _seed = seed;

            _thisWeeksRecipes = new List<CookingRecipe>();
            var maxNumberOfRecipesPerWeek = 5;
            var unknownRecipes = _cookingRecipes.Where(x => !x.IsKnown).ToList();
            var unknownRecipesCount = unknownRecipes.Count;

            if (unknownRecipesCount == 0)
            {
                ShowNoRecipes();
                return;
            }

            for (int i = 0; i < maxNumberOfRecipesPerWeek; i++)
            {
                var recipe = unknownRecipes[random.Next(unknownRecipesCount)];
                if (!_thisWeeksRecipes.Any(x => x.Name.Equals(recipe.Name)))
                {
                    _thisWeeksRecipes.Add(recipe);
                }
            }

            ShowWeeklyRecipes();
        }

        private void ShowNoRecipes()
        {
            Monitor.Log($"No recipes availabe. You know them all.", LogLevel.Info);
        }

        private void ShowWeeklyRecipes()
        {
            if (_thisWeeksRecipes.Count == 0)
            {
                ShowNoRecipes();
                return;
            }

            Monitor.Log($"This weeks recipes are:", LogLevel.Alert);
            foreach (var item in _thisWeeksRecipes)
            {
                Monitor.Log($"{ModHelper.GetMoneyAsString(item.AquisitionConditions.Cost)} - {item.Name}", LogLevel.Info);
            }
        }

        private void DiscoverRecipes()
        {
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
        }

        private void LogUsageError(string error, string command)
        {
            Monitor.Log($"{error} Type 'help {command}' for usage.", LogLevel.Error);
        }

        private void LogArgumentsInvalid(string command)
        {
            LogUsageError("The arguments are invalid.", command);
        }
    }
}
