using MBC.Core.Matches;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MBC.Shared;

namespace CoreUnitTests
{
    
    
    /// <summary>
    ///This is a test class for ClassicGameTest and is intended
    ///to contain all ClassicGameTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ClassicGameTest
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
        ///A test for ClassicGame Constructor
        ///</summary>
        [TestMethod()]
        public void ClassicGameConstructorTest()
        {
            IDNumber roundID = new IDNumber(); // TODO: Initialize to an appropriate value
            ActiveMatch match = null; // TODO: Initialize to an appropriate value
            ClassicGame target = new ClassicGame(roundID, match);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for NextTurn
        ///</summary>
        [TestMethod()]
        public void NextTurnTest()
        {
            IDNumber roundID = new IDNumber(); // TODO: Initialize to an appropriate value
            ActiveMatch match = null; // TODO: Initialize to an appropriate value
            ClassicGame target = new ClassicGame(roundID, match); // TODO: Initialize to an appropriate value
            IDNumber expected = new IDNumber(); // TODO: Initialize to an appropriate value
            IDNumber actual;
            actual = target.NextTurn();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Play
        ///</summary>
        [TestMethod()]
        public void PlayTest()
        {
            IDNumber roundID = new IDNumber(); // TODO: Initialize to an appropriate value
            ActiveMatch match = null; // TODO: Initialize to an appropriate value
            ClassicGame target = new ClassicGame(roundID, match); // TODO: Initialize to an appropriate value
            target.Play();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ShipsValid
        ///</summary>
        [TestMethod()]
        public void ShipsValidTest()
        {
            IDNumber roundID = new IDNumber(); // TODO: Initialize to an appropriate value
            ActiveMatch match = null; // TODO: Initialize to an appropriate value
            ClassicGame target = new ClassicGame(roundID, match); // TODO: Initialize to an appropriate value
            IDNumber player = new IDNumber(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ShipsValid(player);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ShotValid
        ///</summary>
        [TestMethod()]
        public void ShotValidTest()
        {
            IDNumber roundID = new IDNumber(); // TODO: Initialize to an appropriate value
            ActiveMatch match = null; // TODO: Initialize to an appropriate value
            ClassicGame target = new ClassicGame(roundID, match); // TODO: Initialize to an appropriate value
            IDNumber shooter = new IDNumber(); // TODO: Initialize to an appropriate value
            Shot shot = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ShotValid(shooter, shot);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Stop
        ///</summary>
        [TestMethod()]
        public void StopTest()
        {
            IDNumber roundID = new IDNumber(); // TODO: Initialize to an appropriate value
            ActiveMatch match = null; // TODO: Initialize to an appropriate value
            ClassicGame target = new ClassicGame(roundID, match); // TODO: Initialize to an appropriate value
            target.Stop();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Ended
        ///</summary>
        [TestMethod()]
        public void EndedTest()
        {
            IDNumber roundID = new IDNumber(); // TODO: Initialize to an appropriate value
            ActiveMatch match = null; // TODO: Initialize to an appropriate value
            ClassicGame target = new ClassicGame(roundID, match); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Ended;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
