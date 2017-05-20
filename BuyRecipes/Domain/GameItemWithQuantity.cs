using Denifia.Stardew.BuyRecipes.Framework;
using System.Linq;

namespace Denifia.Stardew.BuyRecipes.Domain
{
    /// <summary>
    /// Immutable GameItem with a quantity.
    /// </summary>
    internal class GameItemWithQuantity : GameItem
    {
        /// <summary>
        /// Deserialises a GameItemWithQuantity from a string.
        /// </summary>
        /// <param name="data">The serialised GameItemWithQuantity.</param>
        /// <returns>The deserialised GameItemWithQuantity.</returns>
        public static new GameItemWithQuantity Deserialise(string data)
        {
            var dataParts = data.Split(' ');
            if (dataParts.Length != 2) return null;
            if (!int.TryParse(dataParts[0], out int id)) return null;
            if (!int.TryParse(dataParts[1], out int quantity)) return null;

            var gameItem = ModHelper.GameObjects.FirstOrDefault(x => x.Id == id);
            if (gameItem == null) return null;

            return new GameItemWithQuantity(id, gameItem.Name, quantity);
        }

        private int _quantity;

        public int Quantity => _quantity;

        public GameItemWithQuantity(int id, string name, int quantity) : base(id, name)
        {
            _quantity = quantity;
        }
    }
}
