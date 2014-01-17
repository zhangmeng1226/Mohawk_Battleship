using MBC.Core.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CoreUnitTests
{
    
    
    /// <summary>
    ///This is a test class for ConfigurationTest and is intended
    ///to contain all ConfigurationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConfigurationTest
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
        ///A test for Configuration Constructor
        ///</summary>
        [TestMethod()]
        public void ConfigurationConstructorTest()
        {
            string name = string.Empty; // TODO: Initialize to an appropriate value
            Configuration target = new Configuration(name);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for AddDefault
        ///</summary>
        [TestMethod()]
        public void AddDefaultTest()
        {
            ConfigurationAttribute defAttrib = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Configuration.AddDefault(defAttrib);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FileExists
        ///</summary>
        [TestMethod()]
        public void FileExistsTest()
        {
            Configuration target = new Configuration(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.FileExists();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAllKnownKeys
        ///</summary>
        [TestMethod()]
        public void GetAllKnownKeysTest()
        {
            List<string> expected = null; // TODO: Initialize to an appropriate value
            List<string> actual;
            actual = Configuration.GetAllKnownKeys();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAvailableConfigs
        ///</summary>
        [TestMethod()]
        public void GetAvailableConfigsTest()
        {
            IEnumerable<string> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<string> actual;
            actual = Configuration.GetAvailableConfigs();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetConfigValueArray
        ///</summary>
        public void GetConfigValueArrayTestHelper<T>()
        {
            Configuration target = new Configuration(); // TODO: Initialize to an appropriate value
            string key = string.Empty; // TODO: Initialize to an appropriate value
            List<T> expected = null; // TODO: Initialize to an appropriate value
            List<T> actual;
            actual = target.GetConfigValueArray<T>(key);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void GetConfigValueArrayTest()
        {
            GetConfigValueArrayTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetDefaultValue
        ///</summary>
        public void GetDefaultValueTestHelper<T>()
        {
            string key = string.Empty; // TODO: Initialize to an appropriate value
            T expected = default(T); // TODO: Initialize to an appropriate value
            T actual;
            actual = Configuration.GetDefaultValue<T>(key);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void GetDefaultValueTest()
        {
            GetDefaultValueTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetDescription
        ///</summary>
        [TestMethod()]
        public void GetDescriptionTest()
        {
            string key = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Configuration.GetDescription(key);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetDisplayName
        ///</summary>
        [TestMethod()]
        public void GetDisplayNameTest()
        {
            string key = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Configuration.GetDisplayName(key);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetList
        ///</summary>
        public void GetListTestHelper<T>()
        {
            Configuration target = new Configuration(); // TODO: Initialize to an appropriate value
            string key = string.Empty; // TODO: Initialize to an appropriate value
            List<T> expected = null; // TODO: Initialize to an appropriate value
            List<T> actual;
            actual = target.GetList<T>(key);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void GetListTest()
        {
            GetListTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetPairs
        ///</summary>
        [TestMethod()]
        public void GetPairsTest()
        {
            Configuration target = new Configuration(); // TODO: Initialize to an appropriate value
            List<KeyValuePair<string, object>> expected = null; // TODO: Initialize to an appropriate value
            List<KeyValuePair<string, object>> actual;
            actual = target.GetPairs();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetValue
        ///</summary>
        public void GetValueTestHelper<T>()
        {
            Configuration target = new Configuration(); // TODO: Initialize to an appropriate value
            string key = string.Empty; // TODO: Initialize to an appropriate value
            T expected = default(T); // TODO: Initialize to an appropriate value
            T actual;
            actual = target.GetValue<T>(key);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void GetValueTest()
        {
            GetValueTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetValueString
        ///</summary>
        [TestMethod()]
        public void GetValueStringTest()
        {
            Configuration target = new Configuration(); // TODO: Initialize to an appropriate value
            string key = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetValueString(key);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Initialize
        ///</summary>
        [TestMethod()]
        public void InitializeTest()
        {
            string appDataPath = string.Empty; // TODO: Initialize to an appropriate value
            Configuration.Initialize(appDataPath);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ParseString
        ///</summary>
        [TestMethod()]
        public void ParseStringTest()
        {
            Type valueType = null; // TODO: Initialize to an appropriate value
            string value = string.Empty; // TODO: Initialize to an appropriate value
            object expected = null; // TODO: Initialize to an appropriate value
            object actual;
            actual = Configuration.ParseString(valueType, value);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SaveConfigFile
        ///</summary>
        [TestMethod()]
        public void SaveConfigFileTest()
        {
            Configuration target = new Configuration(); // TODO: Initialize to an appropriate value
            target.SaveConfigFile();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetValue
        ///</summary>
        [TestMethod()]
        public void SetValueTest()
        {
            Configuration target = new Configuration(); // TODO: Initialize to an appropriate value
            string key = string.Empty; // TODO: Initialize to an appropriate value
            string val = string.Empty; // TODO: Initialize to an appropriate value
            target.SetValue(key, val);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Global
        ///</summary>
        [TestMethod()]
        public void GlobalTest()
        {
            Configuration expected = null; // TODO: Initialize to an appropriate value
            Configuration actual;
            Configuration.Global = expected;
            actual = Configuration.Global;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            Configuration target = new Configuration(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Name = expected;
            actual = target.Name;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
