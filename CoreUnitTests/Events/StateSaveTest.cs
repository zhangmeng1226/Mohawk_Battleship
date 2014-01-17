using MBC.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CoreUnitTests
{
    
    
    /// <summary>
    ///This is a test class for StateSaveTest and is intended
    ///to contain all StateSaveTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StateSaveTest
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
        ///A test for StateSave Constructor
        ///</summary>
        [TestMethod()]
        public void StateSaveConstructorTest()
        {
            int idx = 0; // TODO: Initialize to an appropriate value
            StateSave target = new StateSave(idx);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ObjectCopy
        ///</summary>
        [TestMethod()]
        public void ObjectCopyTest()
        {
            int idx = 0; // TODO: Initialize to an appropriate value
            StateSave target = new StateSave(idx); // TODO: Initialize to an appropriate value
            object expected = null; // TODO: Initialize to an appropriate value
            object actual;
            target.ObjectCopy = expected;
            actual = target.ObjectCopy;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
