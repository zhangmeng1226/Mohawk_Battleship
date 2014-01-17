using MBC.Core.Matches;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MBC.Core.Util;

namespace CoreUnitTests
{
    
    
    /// <summary>
    ///This is a test class for AllRoundsMatchTest and is intended
    ///to contain all AllRoundsMatchTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AllRoundsMatchTest
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
        ///A test for AllRoundsMatch Constructor
        ///</summary>
        [TestMethod()]
        public void AllRoundsMatchConstructorTest()
        {
            AllRoundsMatch target = new AllRoundsMatch();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for AllRoundsMatch Constructor
        ///</summary>
        [TestMethod()]
        public void AllRoundsMatchConstructorTest1()
        {
            Configuration conf = null; // TODO: Initialize to an appropriate value
            AllRoundsMatch target = new AllRoundsMatch(conf);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Play
        ///</summary>
        [TestMethod()]
        public void PlayTest()
        {
            AllRoundsMatch target = new AllRoundsMatch(); // TODO: Initialize to an appropriate value
            target.Play();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Stop
        ///</summary>
        [TestMethod()]
        public void StopTest()
        {
            AllRoundsMatch target = new AllRoundsMatch(); // TODO: Initialize to an appropriate value
            target.Stop();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Ended
        ///</summary>
        [TestMethod()]
        public void EndedTest()
        {
            AllRoundsMatch target = new AllRoundsMatch(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Ended;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
