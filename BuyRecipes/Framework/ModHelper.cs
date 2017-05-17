using Denifia.Stardew.BuyRecipes.Domain;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Denifia.Stardew.BuyRecipes.Framework
{
    internal static class ModHelper
    {
        private static List<GameItem> _gameObjects = new List<GameItem>();
        public static List<GameItem> GameObjects => _gameObjects ?? (_gameObjects = DeserializeGameObjects().ToList());

        private static IEnumerable<GameItem> DeserializeGameObjects()
        {
            foreach (var item in Game1.objectInformation)
            {
                yield return new GameItem(id: item.Key, name: item.Value.Split('/')[4]);
            }
        }

        public static string GetMoneyAsString(int money) => $"G{money.ToString("#,##0")}";

        public static void HandleError(IMod mod, Exception ex, string verb)
        {
            mod.Monitor.Log($"Something went wrong {verb}:\n{ex}", LogLevel.Error);
            ModHelper.ShowErrorMessage($"Huh. Something went wrong {verb}. The error log has the technical details.");
        }

        /// <summary>Show an informational message to the player.</summary>
        /// <param name="message">The message to show.</param>
        /// <param name="duration">The number of milliseconds during which to keep the message on the screen before it fades (or <c>null</c> for the default time).</param>
        public static void ShowInfoMessage(string message, int? duration = null)
        {
            Game1.addHUDMessage(new HUDMessage(message, 3) { noIcon = true, timeLeft = duration ?? HUDMessage.defaultTime });
        }

        /// <summary>Show an error message to the player.</summary>
        /// <param name="message">The message to show.</param>
        public static void ShowErrorMessage(string message)
        {
            Game1.addHUDMessage(new HUDMessage(message, 3));
        }
    }
}
