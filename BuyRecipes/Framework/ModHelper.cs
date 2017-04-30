using Denifia.Stardew.BuyRecipes.Domain;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.BuyRecipes.Framework
{
    public static class ModHelper
    {
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
}
