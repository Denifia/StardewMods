using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StardewModdingAPI;

namespace BuyRecipes.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var monitorMock = new Mock<IMonitor>();
            var mod = new Denifia.Stardew.BuyRecipes.BuyRecipes();
        }
    }
}
