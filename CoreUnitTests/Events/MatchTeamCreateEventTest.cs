using MBC.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MBC.Shared;
using System.Runtime.Serialization;

namespace CoreUnitTests
{
    
    
    /// <summary>
    ///This is a test class for MatchTeamCreateEventTest and is intended
    ///to contain all MatchTeamCreateEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MatchTeamCreateEventTest
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
        ///A test for MatchTeamCreateEvent Constructor
        ///</summary>
        [TestMethod()]
        public void MatchTeamCreateEventConstructorTest()
        {
            Team team = null; // TODO: Initialize to an appropriate value
            MatchTeamCreateEvent target = new MatchTeamCreateEvent(team);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for MatchTeamCreateEvent Constructor
        ///</summary>
        [TestMethod()]
        public void MatchTeamCreateEventConstructorTest1()
        {
            SerializationInfo info = null; // TODO: Initialize to an appropriate value
            StreamingContext context = new StreamingContext(); // TODO: Initialize to an appropriate value
            MatchTeamCreateEvent target = new MatchTeamCreateEvent(info, context);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for GetObjectData
        ///</summary>
        [TestMethod()]
        public void GetObjectDataTest()
        {
            Team team = null; // TODO: Initialize to an appropriate value
            MatchTeamCreateEvent target = new MatchTeamCreateEvent(team); // TODO: Initialize to an appropriate value
            SerializationInfo info = null; // TODO: Initialize to an appropriate value
            StreamingContext context = new StreamingContext(); // TODO: Initialize to an appropriate value
            target.GetObjectData(info, context);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Team
        ///</summary>
        [TestMethod()]
        public void TeamTest()
        {
            Team team = null; // TODO: Initialize to an appropriate value
            MatchTeamCreateEvent target = new MatchTeamCreateEvent(team); // TODO: Initialize to an appropriate value
            Team actual;
            actual = target.Team;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
