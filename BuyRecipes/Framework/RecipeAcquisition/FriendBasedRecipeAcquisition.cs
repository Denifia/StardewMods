namespace Denifia.Stardew.BuyRecipes.Framework.RecipeAcquisition
{
    public class FriendBasedRecipeAcquisition : BaseRecipeAcquisition
    {
        private int _friendLevel;
        private string _friend;
        public override int Cost => _friendLevel * 600;

        public FriendBasedRecipeAcquisition() { }

        public FriendBasedRecipeAcquisition(string data) : base(data)
        {
            var dataParts = data.Split(' ');
            _friend = dataParts[1];
            _friendLevel = int.Parse(dataParts[2]);
        }

        public override bool AcceptsConditions(string condition) => condition.StartsWith("f ");
    }
}
