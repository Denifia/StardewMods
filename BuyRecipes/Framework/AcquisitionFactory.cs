using Denifia.Stardew.BuyRecipes.Framework.RecipeAcquisition;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Denifia.Stardew.BuyRecipes.Framework
{
    public class AcquisitionFactory
    {
        /*********
        ** Singleton
        *********/
     
        private static AcquisitionFactory _instance;
        private AcquisitionFactory() { }
        public static AcquisitionFactory Instance
        {
            get => _instance ?? (_instance = new AcquisitionFactory());
        }


        /*********
        ** Properties
        *********/

        /// <summary>Collection of ways that recipes can be acquired.</summary>
        private readonly List<IRecipeAcquisition> _conditions = new List<IRecipeAcquisition>()
        {
            new FriendBasedRecipeAcquisition(),
            new SkillBasedRecipeAcquisition(),
            new LevelBasedRecipeAcquisition()
        };


        /*********
        ** Public methods
        *********/

        public IRecipeAcquisition GetAquisitionImplementation(string conditions)
        {
            var chosenConditionType = _conditions.FirstOrDefault(x => x.AcceptsConditions(conditions)) ?? new BaseRecipeAcquisition();
            return (IRecipeAcquisition)Activator.CreateInstance(chosenConditionType.GetType(), new object[] { conditions });
        }
    }
}
