using MBC.Core.Matches;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CoreUnitTests
{
    
    
    /// <summary>
    ///This is a test class for MatchReplayTest and is intended
    ///to contain all MatchReplayTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MatchReplayTest
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
        ///A test for MatchReplay Constructor
        ///</summary>
        [TestMethod()]
        public void MatchReplayConstructorTest()
        {
            MatchReplay target = new MatchReplay();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Play
        ///</summary>
        [TestMethod()]
        public void PlayTest()
        {
            MatchReplay target = new MatchReplay(); // TODO: Initialize to an appropriate value
            target.Play();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SaveToFile
        ///</summary>
        [TestMethod()]
        public void SaveToFileTest()
        {
            MatchReplay target = new MatchReplay(); // TODO: Initialize to an appropriate value
            string location = string.Empty; // TODO: Initialize to an appropriate value
            target.SaveToFile(location);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Stop
        ///</summary>
        [TestMethod()]
        public void StopTest()
        {
            MatchReplay target = new MatchReplay(); // TODO: Initialize to an appropriate value
            target.Stop();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
