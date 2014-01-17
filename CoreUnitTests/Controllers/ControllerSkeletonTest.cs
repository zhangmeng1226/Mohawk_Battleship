using MBC.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MBC.Shared;
using System.Collections.Generic;

namespace CoreUnitTests
{
    
    
    /// <summary>
    ///This is a test class for ControllerSkeletonTest and is intended
    ///to contain all ControllerSkeletonTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ControllerSkeletonTest
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
        ///A test for CreateInstance
        ///</summary>
        [TestMethod()]
        public void CreateInstanceTest()
        {
            ControllerSkeleton_Accessor target = new ControllerSkeleton_Accessor(); // TODO: Initialize to an appropriate value
            Controller expected = null; // TODO: Initialize to an appropriate value
            Controller actual;
            actual = target.CreateInstance();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAttribute
        ///</summary>
        public void GetAttributeTestHelper<T>()
        {
            ControllerSkeleton_Accessor target = new ControllerSkeleton_Accessor(); // TODO: Initialize to an appropriate value
            T expected = default(T); // TODO: Initialize to an appropriate value
            T actual;
            actual = target.GetAttribute<T>();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void GetAttributeTest()
        {
            GetAttributeTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for LoadControllerDLL
        ///</summary>
        [TestMethod()]
        public void LoadControllerDLLTest()
        {
            string filePath = string.Empty; // TODO: Initialize to an appropriate value
            List<ControllerSkeleton> expected = null; // TODO: Initialize to an appropriate value
            List<ControllerSkeleton> actual;
            actual = ControllerSkeleton.LoadControllerDLL(filePath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadControllerFolder
        ///</summary>
        [TestMethod()]
        public void LoadControllerFolderTest()
        {
            string path = string.Empty; // TODO: Initialize to an appropriate value
            List<ControllerSkeleton> expected = null; // TODO: Initialize to an appropriate value
            List<ControllerSkeleton> actual;
            actual = ControllerSkeleton.LoadControllerFolder(path);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            ControllerSkeleton_Accessor target = new ControllerSkeleton_Accessor(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Controller
        ///</summary>
        [TestMethod()]
        public void ControllerTest()
        {
            ControllerSkeleton_Accessor target = new ControllerSkeleton_Accessor(); // TODO: Initialize to an appropriate value
            Type actual;
            actual = target.Controller;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DLLFileName
        ///</summary>
        [TestMethod()]
        public void DLLFileNameTest()
        {
            ControllerSkeleton_Accessor target = new ControllerSkeleton_Accessor(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.DLLFileName;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
