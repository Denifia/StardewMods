using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StardewModdingApi;
using Moq;
using Denifia.Stardew.TestableMod;

namespace TestableMod.Test
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
        }

        [TestMethod]
        public void TestMethod1()
        {
            var monitorMoq = new Mock<IMonitor>();
            var testableMod = new Denifia.Stardew.TestableMod.TestableMod(monitorMoq.Object);
        }
    }
}
