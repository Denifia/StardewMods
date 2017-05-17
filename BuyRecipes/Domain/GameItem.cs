namespace Denifia.Stardew.BuyRecipes.Domain
{
    /// <summary>
    /// Immutable Game Item.
    /// </summary>
    internal class GameItem
    {
        private int _id;
        public int Id => _id;

        private string _name;
        public string Name => _name;

        public GameItem(int id, string name)
        {
            _id = id;
            _name = name;
        }
    }
}
