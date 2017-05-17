namespace Denifia.Stardew.BuyRecipes.Domain
{
    /// <summary>
    /// Immutable Game Item with a quantity.
    /// </summary>
    public class GameItemWithQuantity : GameItem
    {
        private int _quantity;
        public int Quantity => _quantity;

        public GameItemWithQuantity(int id, string name, int quantity) 
            : base(id, name)
        {
            _quantity = quantity;
        }
    }
}
