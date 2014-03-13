using MBC.Core.Game;
using MBC.Core.Util;
using MBC.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestCore
{
    [TestClass]
    public class MatchTest
    {
        private Match match;

        public MatchTest()
        {
        }

        [TestInitialize]
        public void Initialize()
        {
            Configuration.Initialize(Environment.CurrentDirectory);
        }

        [TestMethod]
        public bool PlayerDisqualifiedTest()
        {
            var matchTest = new Match(Configuration.Global);
            var testPlayer = new Player(0, "Test Player");

            matchTest.PlayerAdd(testPlayer);

            Assert.IsTrue(matchTest.PlayerDisqualify(testPlayer, "Test reason"));

            matchTest.RemovePlayer(testPlayer);

            Assert.IsFalse(matchTest.PlayerDisqualify(testPlayer, "Test reason"));
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}