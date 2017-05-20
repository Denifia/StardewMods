﻿using Denifia.Stardew.BuyRecipes.Framework;
using System.Linq;

namespace Denifia.Stardew.BuyRecipes.Domain
{
    /// <summary>
    /// Immutable GameItem.
    /// </summary>
    internal class GameItem
    {
        /// <summary>
        /// Deserialises a GameItem from a string.
        /// </summary>
        /// <param name="data">The serialised GameItem.</param>
        /// <returns>The deserialised GameItem.</returns>
        public static GameItem Deserialise(string data)
        {
            var dataParts = data.Split(' ');
            if (dataParts.Length != 1) return null;
            if (!int.TryParse(dataParts[0], out int id)) return null;

            var gameItem = ModHelper.GameObjects.FirstOrDefault(x => x.Id == id);
            if (gameItem == null) return null;

            return new GameItem(id, gameItem.Name);
        }

        private int _id;
        private string _name;

        public int Id => _id;
        public string Name => _name;

        public GameItem(int id, string name)
        {
            _id = id;
            _name = name;
        }
    }
}
