using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBC.Core.Util;
using System.Collections.Generic;
using System.Collections;

namespace TestCore
{
    [Configuration("test_int", 5)]
    [Configuration("test_str", "test")]
    [Configuration("test_enum", ConsoleColor.DarkBlue)]
    [Configuration("test_decimal", 4.2d)]
    [Configuration("test_list_double", 4.2d, 4.3d, 4.4d, 4.5d, 4.6d)]
    [Configuration("test_list_single", 5, null)]
    [Configuration("test_naming", 5, Description="Test Description", DisplayName="TestName")]
    [TestClass]
    public class ConfigurationTest
    {
        Configuration testConfig;

        [ClassInitialize]
        public static void ConfigurationInitialize(TestContext context)
        {
            Configuration.Initialize(Environment.CurrentDirectory);
        }

        [TestInitialize]
        public void ConfigurationCreate()
        {
            testConfig = new Configuration("test");
        }

        [TestMethod]
        public void Configuration_DefaultKeys_Loaded()
        {
            List<string> myKeys = new List<string>() { "test_int", "test_str", "test_enum", "test_decimal", "test_list_double", "test_list_single", "test_naming" };
            foreach (string key in Configuration.GetAllKnownKeys())
            {
                myKeys.Remove(key);
            }
            Assert.AreEqual(myKeys.Count, 0);
        }

        [TestMethod]
        public void Configuration_DefaultIntVal_Gets()
        {
            Assert.AreEqual(Configuration.GetDefaultValue<int>("test_int"), 5);
        }

        [TestMethod]
        public void Configuration_DefaultStringVal_Gets()
        {
            Assert.AreEqual(Configuration.GetDefaultValue<string>("test_str"), "test");
        }

        [TestMethod]
        public void Configuration_DefaultEnumVal_Gets()
        {
            Assert.AreEqual(Configuration.GetDefaultValue<ConsoleColor>("test_enum"), ConsoleColor.DarkBlue);
        }

        [TestMethod]
        public void Configuration_DefaultDoubleVal_Gets()
        {
            Assert.AreEqual(Configuration.GetDefaultValue<double>("test_decimal"), 4.2d);
        }

        [TestMethod]
        public void Configuration_DefaultDoubleList_Gets()
        {
            List<double> myDoubles = Configuration.GetDefaultList<double>("test_list_double");
            Assert.IsTrue(myDoubles[0] == 4.2d && 
                myDoubles[1] == 4.3d && 
                myDoubles[2] == 4.4d && 
                myDoubles[3] == 4.5d && 
                myDoubles[4] == 4.6d);
        }

        [TestMethod]
        public void Configuration_DefaultSingleIntList_Gets()
        {
            List<int> myInt = Configuration.GetDefaultList<int>("test_list_single");
            Assert.AreEqual(myInt[0], 5);
        }

        [TestMethod]
        public void Configuration_DefaultList_CountProper()
        {
            Assert.AreEqual(Configuration.GetDefaultList<double>("test_list_double").Count, 5);
        }

        [TestMethod]
        public void Configuration_SetGetKeyValue_Works()
        {
            testConfig.SetValue("test_decimal", 400.3d);
            Assert.IsTrue(testConfig.GetValue<double>("test_decimal") == 400.3d);
        }

        [TestMethod]
        public void Configuration_SetGetKeyValueString_Works()
        {
            testConfig.SetValue("test_int", "20");
            Assert.AreEqual(testConfig.GetValue("test_int"), "20");
        }
        
        [TestMethod]
        public void Configuration_SetGetArrayList_Works()
        {
            var doubleVals = new ArrayList() {2d, 3d, 4d, 5.1d, 5.69d};
            testConfig.SetValue("test_list_double", doubleVals);
            var configVals = testConfig.GetValue<ArrayList>("test_list_double");
            bool result = true;
            for (int i = 0; i < doubleVals.Count; i++)
            {
                result &= configVals[i] == doubleVals[i];
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Configuration_DefaultDescription_Gets()
        {
            Assert.AreEqual(Configuration.GetDescription("test_naming"), "Test Description");
        }

        [TestMethod]
        public void Configuration_DefaultName_Gets()
        {
            Assert.AreEqual(Configuration.GetDisplayName("test_naming"), "TestName");
        }

    }
}
