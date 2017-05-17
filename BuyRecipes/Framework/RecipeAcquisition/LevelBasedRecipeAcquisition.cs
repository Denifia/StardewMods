namespace Denifia.Stardew.BuyRecipes.Framework.RecipeAcquisition
{
    public class LevelBasedRecipeAcquisition : BaseRecipeAcquisition
    {
        private int _playerLevel;
        public override int Cost => GetCost();

        public LevelBasedRecipeAcquisition() { }

        public LevelBasedRecipeAcquisition(string data) : base(data)
        {
            var dataParts = data.Split(' ');
            _playerLevel = int.Parse(dataParts[1]);
        }

        public override bool AcceptsConditions(string condition) => condition.StartsWith("l ");

        private int GetCost()
        {
            if (_playerLevel == 100) return 1750;
            return _playerLevel * 75;
        }
    }
}
