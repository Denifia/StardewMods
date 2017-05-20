namespace Denifia.Stardew.BuyRecipes.Domain
{
    internal class CookingRecipeData
    {
        public static CookingRecipeData Deserialise(string data)
        {
            var dataParts = data.Split('/');
            var ingredientsData = dataParts[0];
            var unknownData = dataParts[1];
            var resultingItemData = dataParts[2];
            var acquisitionData = dataParts[3];

            return new CookingRecipeData(ingredientsData, resultingItemData, acquisitionData);
        }

        private string _ingredientsData;
        private string _resultingItemData;
        private string _acquisitionData;

        public string IngredientsData => _ingredientsData;
        public string ResultingItemData => _resultingItemData;
        public string AcquisitionData => _acquisitionData;

        private CookingRecipeData(string ingredientsData, string resultingItemData, string acquisitionData)
        {
            _ingredientsData = ingredientsData;
            _resultingItemData = resultingItemData;
            _acquisitionData = acquisitionData;
        }
    }
}
