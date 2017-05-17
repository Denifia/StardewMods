using Denifia.Stardew.BuyRecipes.Framework.RecipeAcquisition;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Denifia.Stardew.BuyRecipes.Framework
{
    public class AquisitionFactory
    {
        /*********
        ** Singleton
        *********/
     
        private static AquisitionFactory _instance;
        private AquisitionFactory() { }
        public static AquisitionFactory Instance
        {
            get => _instance ?? (_instance = new AquisitionFactory());
        }


        /*********
        ** Properties
        *********/

        /// <summary>Collection of ways that recipes can be aquired.</summary>
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
