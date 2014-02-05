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
        public void PlayerDisqualifiedTest()
        {
            var matchTest = new Match(Configuration.Global);
            var testPlayer = new Player(0, "Test Player");

            matchTest.AddPlayer(testPlayer);

            Assert.IsTrue(matchTest.PlayerDisqualified(testPlayer, "Test reason"));

            matchTest.RemovePlayer(testPlayer);

            Assert.IsFalse(matchTest.PlayerDisqualified(testPlayer, "Test reason"));
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}