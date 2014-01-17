using MBC.Core.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MBC.Shared;
using System.Collections.Generic;

namespace CoreUnitTests
{
    
    
    /// <summary>
    ///This is a test class for TimedControllerTest and is intended
    ///to contain all TimedControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TimedControllerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for PlaceShips
        ///</summary>
        [TestMethod()]
        public void PlaceShipsTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            ShipList expected = null; // TODO: Initialize to an appropriate value
            ShipList actual;
            actual = target.PlaceShips();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for OpponentShot
        ///</summary>
        [TestMethod()]
        public void OpponentShotTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            Shot shot = null; // TODO: Initialize to an appropriate value
            target.OpponentShot(shot);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OpponentDestroyed
        ///</summary>
        [TestMethod()]
        public void OpponentDestroyedTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            IDNumber destroyedID = new IDNumber(); // TODO: Initialize to an appropriate value
            target.OpponentDestroyed(destroyedID);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for NewRound
        ///</summary>
        [TestMethod()]
        public void NewRoundTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            target.NewRound();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for NewMatch
        ///</summary>
        [TestMethod()]
        public void NewMatchTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            target.NewMatch();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for MatchOver
        ///</summary>
        [TestMethod()]
        public void MatchOverTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            target.MatchOver();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for MakeShot
        ///</summary>
        [TestMethod()]
        public void MakeShotTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            Shot expected = null; // TODO: Initialize to an appropriate value
            Shot actual;
            actual = target.MakeShot();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAttribute
        ///</summary>
        public void GetAttributeTestHelper<T>()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            T expected = default(T); // TODO: Initialize to an appropriate value
            T actual;
            actual = target.GetAttribute<T>();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void GetAttributeTest()
        {
            GetAttributeTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for TimedController Constructor
        ///</summary>
        [TestMethod()]
        public void TimedControllerConstructorTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for RoundLost
        ///</summary>
        [TestMethod()]
        public void RoundLostTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            target.RoundLost();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RoundWon
        ///</summary>
        [TestMethod()]
        public void RoundWonTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            target.RoundWon();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ShotHit
        ///</summary>
        [TestMethod()]
        public void ShotHitTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            Shot shot = null; // TODO: Initialize to an appropriate value
            bool sunk = false; // TODO: Initialize to an appropriate value
            target.ShotHit(shot, sunk);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ShotMiss
        ///</summary>
        [TestMethod()]
        public void ShotMissTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            Shot shot = null; // TODO: Initialize to an appropriate value
            target.ShotMiss(shot);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Field
        ///</summary>
        [TestMethod()]
        public void FieldTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            FieldInfo expected = null; // TODO: Initialize to an appropriate value
            FieldInfo actual;
            target.Field = expected;
            actual = target.Field;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ID
        ///</summary>
        [TestMethod()]
        public void IDTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            IDNumber expected = new IDNumber(); // TODO: Initialize to an appropriate value
            IDNumber actual;
            target.ID = expected;
            actual = target.ID;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Match
        ///</summary>
        [TestMethod()]
        public void MatchTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            MatchConfig expected = null; // TODO: Initialize to an appropriate value
            MatchConfig actual;
            target.Match = expected;
            actual = target.Match;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Registers
        ///</summary>
        [TestMethod()]
        public void RegistersTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            Dictionary<IDNumber, Register> expected = null; // TODO: Initialize to an appropriate value
            Dictionary<IDNumber, Register> actual;
            target.Registers = expected;
            actual = target.Registers;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Teams
        ///</summary>
        [TestMethod()]
        public void TeamsTest()
        {
            IController controllerWrap = null; // TODO: Initialize to an appropriate value
            TimedController target = new TimedController(controllerWrap); // TODO: Initialize to an appropriate value
            Dictionary<IDNumber, Team> expected = null; // TODO: Initialize to an appropriate value
            Dictionary<IDNumber, Team> actual;
            target.Teams = expected;
            actual = target.Teams;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
