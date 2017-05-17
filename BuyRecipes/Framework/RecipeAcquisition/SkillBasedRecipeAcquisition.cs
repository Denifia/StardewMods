namespace Denifia.Stardew.BuyRecipes.Framework.RecipeAcquisition
{
    public class SkillBasedRecipeAcquisition : BaseRecipeAcquisition
    {
        private int _skillLevel;
        private string _skill;
        public override int Cost => _skillLevel * 900;

        public SkillBasedRecipeAcquisition() { }

        public SkillBasedRecipeAcquisition(string data) : base(data)
        {
            var dataParts = data.Split(' ');
            _skill = dataParts[1];
            _skillLevel = int.Parse(dataParts[2]);
        }

        public override bool AcceptsConditions(string condition) => condition.StartsWith("s ");
    }
}
