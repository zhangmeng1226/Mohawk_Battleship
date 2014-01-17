using MBC.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace CoreUnitTests
{
    
    
    /// <summary>
    ///This is a test class for EventDriverTest and is intended
    ///to contain all EventDriverTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EventDriverTest
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
        ///A test for EventDriver Constructor
        ///</summary>
        [TestMethod()]
        public void EventDriverConstructorTest()
        {
            EventDriver target = new EventDriver();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for EventDriver Constructor
        ///</summary>
        [TestMethod()]
        public void EventDriverConstructorTest1()
        {
            SerializationInfo info = null; // TODO: Initialize to an appropriate value
            StreamingContext context = new StreamingContext(); // TODO: Initialize to an appropriate value
            EventDriver target = new EventDriver(info, context);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for AddEvent
        ///</summary>
        [TestMethod()]
        public void AddEventTest()
        {
            EventDriver target = new EventDriver(); // TODO: Initialize to an appropriate value
            Event ev = null; // TODO: Initialize to an appropriate value
            target.AddEvent(ev);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ApplyEvent
        ///</summary>
        [TestMethod()]
        public void ApplyEventTest()
        {
            EventDriver target = new EventDriver(); // TODO: Initialize to an appropriate value
            Event ev = null; // TODO: Initialize to an appropriate value
            target.ApplyEvent(ev);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetObjectData
        ///</summary>
        [TestMethod()]
        public void GetObjectDataTest()
        {
            EventDriver target = new EventDriver(); // TODO: Initialize to an appropriate value
            SerializationInfo info = null; // TODO: Initialize to an appropriate value
            StreamingContext context = new StreamingContext(); // TODO: Initialize to an appropriate value
            target.GetObjectData(info, context);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Play
        ///</summary>
        [TestMethod()]
        public void PlayTest()
        {
            EventDriver target = new EventDriver(); // TODO: Initialize to an appropriate value
            float timeScale = 0F; // TODO: Initialize to an appropriate value
            target.Play(timeScale);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Play
        ///</summary>
        [TestMethod()]
        public void PlayTest1()
        {
            EventDriver target = new EventDriver(); // TODO: Initialize to an appropriate value
            target.Play();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Stop
        ///</summary>
        [TestMethod()]
        public void StopTest()
        {
            EventDriver target = new EventDriver(); // TODO: Initialize to an appropriate value
            target.Stop();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for AtEnd
        ///</summary>
        [TestMethod()]
        public void AtEndTest()
        {
            EventDriver target = new EventDriver(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.AtEnd;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Events
        ///</summary>
        [TestMethod()]
        public void EventsTest()
        {
            EventDriver target = new EventDriver(); // TODO: Initialize to an appropriate value
            IList<Event> actual;
            actual = target.Events;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
