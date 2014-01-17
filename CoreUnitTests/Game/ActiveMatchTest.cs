using MBC.Core.Matches;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MBC.Shared;
using MBC.Core.Util;
using System.Collections.Generic;

namespace CoreUnitTests
{
    
    
    /// <summary>
    ///This is a test class for ActiveMatchTest and is intended
    ///to contain all ActiveMatchTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ActiveMatchTest
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


        internal virtual ActiveMatch CreateActiveMatch()
        {
            // TODO: Instantiate an appropriate concrete class.
            ActiveMatch target = null;
            return target;
        }

        /// <summary>
        ///A test for AddController
        ///</summary>
        [TestMethod()]
        public void AddControllerTest()
        {
            ActiveMatch target = CreateActiveMatch(); // TODO: Initialize to an appropriate value
            IController plr = null; // TODO: Initialize to an appropriate value
            target.AddController(plr);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for End
        ///</summary>
        [TestMethod()]
        public void EndTest()
        {
            ActiveMatch target = CreateActiveMatch(); // TODO: Initialize to an appropriate value
            target.End();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetTeam
        ///</summary>
        [TestMethod()]
        public void GetTeamTest()
        {
            ActiveMatch target = CreateActiveMatch(); // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            IDNumber expected = new IDNumber(); // TODO: Initialize to an appropriate value
            IDNumber actual;
            actual = target.GetTeam(name);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SaveToFile
        ///</summary>
        [TestMethod()]
        public void SaveToFileTest()
        {
            ActiveMatch target = CreateActiveMatch(); // TODO: Initialize to an appropriate value
            string location = string.Empty; // TODO: Initialize to an appropriate value
            target.SaveToFile(location);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetConfiguration
        ///</summary>
        [TestMethod()]
        public void SetConfigurationTest()
        {
            ActiveMatch target = CreateActiveMatch(); // TODO: Initialize to an appropriate value
            Configuration config = null; // TODO: Initialize to an appropriate value
            target.SetConfiguration(config);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetControllerToTeam
        ///</summary>
        [TestMethod()]
        public void SetControllerToTeamTest()
        {
            ActiveMatch target = CreateActiveMatch(); // TODO: Initialize to an appropriate value
            IDNumber ctrl = new IDNumber(); // TODO: Initialize to an appropriate value
            IDNumber team = new IDNumber(); // TODO: Initialize to an appropriate value
            target.SetControllerToTeam(ctrl, team);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UnsetControllerFromTeam
        ///</summary>
        [TestMethod()]
        public void UnsetControllerFromTeamTest()
        {
            ActiveMatch target = CreateActiveMatch(); // TODO: Initialize to an appropriate value
            IDNumber ctrl = new IDNumber(); // TODO: Initialize to an appropriate value
            IDNumber team = new IDNumber(); // TODO: Initialize to an appropriate value
            target.UnsetControllerFromTeam(ctrl, team);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Controllers
        ///</summary>
        [TestMethod()]
        public void ControllersTest()
        {
            ActiveMatch target = CreateActiveMatch(); // TODO: Initialize to an appropriate value
            IDictionary<IDNumber, IController> actual;
            actual = target.Controllers;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Ended
        ///</summary>
        [TestMethod()]
        public void EndedTest()
        {
            ActiveMatch target = CreateActiveMatch(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Ended;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
