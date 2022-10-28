using DynamicLanguageInterop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestDynamicLanguageInterop
{  
    /// <summary>
    ///This is a test class for InteropTestWindowTest and is intended
    ///to contain all InteropTestWindowTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InteropTestWindowTest
    {
        string[] stringTestData = new string[]{"the", "cat", "sat", "on", "the", "mat", "the", "dog", "had", "a", "bone"};
        int[] intTestData = new int[] { 1, 3, 5, 7, 9, 11, 10, 8, 6, 4, 2, 0, 1, 3, 5, 7, 9, 11 };

        object[,] badTrapezoidTestData = { { 0, 0, 0, 0, "Length of SideAB must be greater than zero" },  // Data + expected exception message
                                           { 90, 15, 10, 20, "VertexA must be less than 90 degrees" },   
                                           { 45, 10, 20, 1, "SideAB must not be shorter than SideCD" },    
                                           { 45, 50, 40, 50, "Height and length of SideCD are too big compared to SideAB" }    
                                      };

        int[,] goodTrapezoidTestData = { { 45, 200, 100, 10, 1500 }, // Fifth value is area 
                                         { 89, 200, 150, 200, 35000 },
                                         { 45, 100, 2, 90, 4590 },
                                         { 10, 200, 90, 2, 290 }
                                       };
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
        }
        
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }
        
        [TestInitialize()]
        public void MyTestInitialize()
        {
        }
        
        [TestCleanup()]
        public void MyTestCleanup()
        {
        }
        
        #endregion


        /// <summary>
        ///A test for ShuffleData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("DynamicLanguageInterop.exe")]
        public void ShuffleDataTest()
        {
            InteropTestWindow_Accessor target = new InteropTestWindow_Accessor();

            // Do string tests first
            object[] data = new string[stringTestData.Length];
            stringTestData.CopyTo(data, 0);
            target.ShuffleData(data);

            // Check that the array still has the same number of elements after shuffling
            Assert.AreEqual(stringTestData.Length, data.Length);

            // Verify that each word occurs the correct number of times
            string[] searchData = null;
            string[] searchStringTestData = null;
            foreach (string word in stringTestData)
            {
                searchData = Array.FindAll(data as string[], (s) => s == word);
                searchStringTestData = Array.FindAll(stringTestData as string[], (s) => s == word);
                Assert.AreEqual(searchData.Length, searchStringTestData.Length);
            }

            // Do int tests
            object[] intData = new object[intTestData.Length];
            intTestData.CopyTo(intData, 0);
            target.ShuffleData(intData);

            // Check that the array still has the same number of elements after shuffling
            Assert.AreEqual(intTestData.Length, intData.Length);

            // Verify that each word occurs the correct number of times
            object[] searchIntData = null;
            int[] searchIntTestData = null;
            foreach (int number in intTestData)
            {
                searchIntData = Array.FindAll(intData, (i) => (int)i == number);
                searchIntTestData = Array.FindAll(intTestData as int[], (i) => i == number);
                Assert.AreEqual(searchIntData.Length, searchIntTestData.Length);
            }      
        }

        /// <summary>
        ///A test for CreateTrapezoid
        ///</summary>
        [TestMethod()]
        [DeploymentItem("DynamicLanguageInterop.exe")]
        public void CreateTrapezoidTest()
        {
            InteropTestWindow_Accessor target = new InteropTestWindow_Accessor();

            // Tests that should not create a trapezoid successfully
            for (int testCount = 0; testCount < badTrapezoidTestData.GetLength(0); testCount++)
            {
                int vertexAInDegrees = (int)badTrapezoidTestData[testCount, 0];
                int lengthSideAB = (int)badTrapezoidTestData[testCount, 1];
                int lengthSideCD = (int)badTrapezoidTestData[testCount, 2];
                int heightOfTrapezoid = (int)badTrapezoidTestData[testCount, 3]; 
                dynamic actual = null;

                try
                {
                    actual = target.CreateTrapezoid(vertexAInDegrees, lengthSideAB, lengthSideCD, heightOfTrapezoid);
                }
                catch (Exception ex)
                {
                    // Verify that the correct exception was raised
                    Assert.AreEqual(ex.Message, badTrapezoidTestData[testCount, 4]);
                }

                // Verify that the trapezoid was not created
                Assert.IsNull(actual);
            }

            // Tests that should successfully create a trapezoid
            for (int testCount = 0; testCount < goodTrapezoidTestData.GetLength(0); testCount++)
            {
                int vertexAInDegrees = goodTrapezoidTestData[testCount, 0];
                int lengthSideAB = goodTrapezoidTestData[testCount, 1];
                int lengthSideCD = goodTrapezoidTestData[testCount, 2];
                int heightOfTrapezoid = goodTrapezoidTestData[testCount, 3];
                dynamic actual = null;
                actual = target.CreateTrapezoid(vertexAInDegrees, lengthSideAB, lengthSideCD, heightOfTrapezoid);
                
                // Verify that the trapezoid was created successfully
                Assert.IsNotNull(actual);

                // Verify that the trapezoid has the expected area
                Assert.AreEqual(actual.area(), goodTrapezoidTestData[testCount, 4]);
            }
        }
    }
}
