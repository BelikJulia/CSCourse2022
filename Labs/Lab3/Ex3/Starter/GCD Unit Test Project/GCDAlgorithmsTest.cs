using GreatestCommonDivisor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GCDTestProject
{


    /// <summary>
    ///This is a test class for GCDAlgorithmsTest and is intended
    ///to contain all GCDAlgorithmsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GCDAlgorithmsTest
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

        // TODO Exercise 3, Task 3
        // Modify all tests to include timing parameter

        /// <summary>
        ///A test for findGCDEuclid
        ///</summary>
        [TestMethod()]
        public void FindGCDEuclidTest()
        {
            int a = 298467352;
            int b = 569484;
            long time;
            int expected = 4;
            int actual;
            actual = GCDAlgorithms.FindGCDEuclid(a, b, out time);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for findGCDEuclid
        ///</summary>
        [TestMethod()]
        public void FindGCDEuclidTest1()
        {
            int a = 298467352;
            int b = 569484;
            int c = 1024;
            int expected = 4;
            int actual;
            actual = GCDAlgorithms.FindGCDEuclid(a, b, c);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for findGCDEuclid
        ///</summary>
        [TestMethod()]
        public void FindGCDEuclidTest2()
        {
            int a = 298467352;
            int b = 569484;
            int c = 1024;
            int d = 109286;
            int expected = 2;
            int actual;
            actual = GCDAlgorithms.FindGCDEuclid(a, b, c, d);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for findGCDEuclid
        ///</summary>
        [TestMethod()]
        public void FindGCDEuclidTest3()
        {
            int a = 298467352;
            int b = 569484;
            int c = 1024;
            int d = 109286;
            int e = 867584396;
            int expected = 2;
            int actual;
            actual = GCDAlgorithms.FindGCDEuclid(a, b, c, d, e);
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod()]
        public void FindGCDSteinTest()
        {
            int u = 298467352;
            int v = 569484;
            long time;
            int expected = 4;
            int actual;
            actual = GCDAlgorithms.FindGCDStein(u, v, out time);
            Assert.AreEqual(expected, actual);
        }

    }
}
