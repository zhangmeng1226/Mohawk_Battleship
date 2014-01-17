using MBC.Core.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CoreUnitTests
{
    
    
    /// <summary>
    ///This is a test class for ConfigurationAttributeTest and is intended
    ///to contain all ConfigurationAttributeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConfigurationAttributeTest
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
        ///A test for ConfigurationAttribute Constructor
        ///</summary>
        [TestMethod()]
        public void ConfigurationAttributeConstructorTest()
        {
            string key = string.Empty; // TODO: Initialize to an appropriate value
            object value = null; // TODO: Initialize to an appropriate value
            ConfigurationAttribute target = new ConfigurationAttribute(key, value);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ConfigurationAttribute Constructor
        ///</summary>
        [TestMethod()]
        public void ConfigurationAttributeConstructorTest1()
        {
            string key = string.Empty; // TODO: Initialize to an appropriate value
            object[] values = null; // TODO: Initialize to an appropriate value
            ConfigurationAttribute target = new ConfigurationAttribute(key, values);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Description
        ///</summary>
        [TestMethod()]
        public void DescriptionTest()
        {
            string key = string.Empty; // TODO: Initialize to an appropriate value
            object[] values = null; // TODO: Initialize to an appropriate value
            ConfigurationAttribute target = new ConfigurationAttribute(key, values); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Description = expected;
            actual = target.Description;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DisplayName
        ///</summary>
        [TestMethod()]
        public void DisplayNameTest()
        {
            string key = string.Empty; // TODO: Initialize to an appropriate value
            object[] values = null; // TODO: Initialize to an appropriate value
            ConfigurationAttribute target = new ConfigurationAttribute(key, values); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DisplayName = expected;
            actual = target.DisplayName;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Key
        ///</summary>
        [TestMethod()]
        public void KeyTest()
        {
            string key = string.Empty; // TODO: Initialize to an appropriate value
            object[] values = null; // TODO: Initialize to an appropriate value
            ConfigurationAttribute target = new ConfigurationAttribute(key, values); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.Key;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Value
        ///</summary>
        [TestMethod()]
        public void ValueTest()
        {
            string key = string.Empty; // TODO: Initialize to an appropriate value
            object[] values = null; // TODO: Initialize to an appropriate value
            ConfigurationAttribute target = new ConfigurationAttribute(key, values); // TODO: Initialize to an appropriate value
            object actual;
            actual = target.Value;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
