using MBC.Core.Matches;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MBC.Core.Events;
using MBC.Shared;
using System.Collections.Generic;

namespace CoreUnitTests
{
    
    
    /// <summary>
    ///This is a test class for MatchTest and is intended
    ///to contain all MatchTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MatchTest
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


        internal virtual Match CreateMatch()
        {
            // TODO: Instantiate an appropriate concrete class.
            Match target = null;
            return target;
        }

        /// <summary>
        ///A test for AddEventAction
        ///</summary>
        [TestMethod()]
        public void AddEventActionTest()
        {
            Match target = CreateMatch(); // TODO: Initialize to an appropriate value
            Type typeOfEvent = null; // TODO: Initialize to an appropriate value
            MBCEventHandler eventAction = null; // TODO: Initialize to an appropriate value
            target.AddEventAction(typeOfEvent, eventAction);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Play
        ///</summary>
        [TestMethod()]
        public void PlayTest()
        {
            Match target = CreateMatch(); // TODO: Initialize to an appropriate value
            target.Play();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RemoveEventAction
        ///</summary>
        [TestMethod()]
        public void RemoveEventActionTest()
        {
            Match target = CreateMatch(); // TODO: Initialize to an appropriate value
            Type typeOfEvent = null; // TODO: Initialize to an appropriate value
            MBCEventHandler eventAction = null; // TODO: Initialize to an appropriate value
            target.RemoveEventAction(typeOfEvent, eventAction);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SaveToFile
        ///</summary>
        [TestMethod()]
        public void SaveToFileTest()
        {
            Match target = CreateMatch(); // TODO: Initialize to an appropriate value
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
            Match target = CreateMatch(); // TODO: Initialize to an appropriate value
            target.Stop();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Fields
        ///</summary>
        [TestMethod()]
        public void FieldsTest()
        {
            Match target = CreateMatch(); // TODO: Initialize to an appropriate value
            IDictionary<IDNumber, FieldInfo> actual;
            actual = target.Fields;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Registers
        ///</summary>
        [TestMethod()]
        public void RegistersTest()
        {
            Match target = CreateMatch(); // TODO: Initialize to an appropriate value
            IDictionary<IDNumber, Register> actual;
            actual = target.Registers;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Teams
        ///</summary>
        [TestMethod()]
        public void TeamsTest()
        {
            Match target = CreateMatch(); // TODO: Initialize to an appropriate value
            IDictionary<IDNumber, Team> actual;
            actual = target.Teams;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
