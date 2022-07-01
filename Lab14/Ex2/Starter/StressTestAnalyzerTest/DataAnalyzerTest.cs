using StressDataAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using StressTestResult;
using System.Collections.Generic;

namespace StressDataAnalyzerTest
{
    /// <summary>
    ///This is a test class for DataAnalyzerTest and is intended
    ///to contain all DataAnalyzerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataAnalyzerTest
    {
        object[,] testQueryCriteria = {{ true, DateTime.MinValue, DateTime.MaxValue, true, (short)100, (short)500, true, (short)10, (short)5000, true, (short)0, (short)1500, 41073, 1065864},
                                       { true, new DateTime(2009, 11, 1), new DateTime(2010, 2, 1), true, (short)280, (short)400, true, (short)10, (short)1000, true, (short)200, (short)750, 1180, 30252},
                                       { true, new DateTime(2009, 8, 1), new DateTime(2010, 3, 31), true, (short)320, (short)320, true, (short)10, (short)5000, true, (short)0, (short)1500, 785, 20377},
                                       { true, new DateTime(2009, 8, 1), new DateTime(2010, 3, 31), true, (short)500, (short)500, true, (short)10, (short)5000, true, (short)0, (short)1500, 829, 21513},
                                       { true, new DateTime(2010, 6, 1), new DateTime(2010, 6, 30), true, (short)200, (short)500, true, (short)10, (short)5000, true, (short)0, (short)1500, 0, 99}
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


        ///<summary>
        ///A test for CreateQuery and FormatResults
        ///</summary>
        [TestMethod()]
        [DeploymentItem("StressDataAnalyzer.exe")]
        public void CreateQueryandFormatResultsTest()
        {
             DataAnalyzer_Accessor target = new DataAnalyzer_Accessor();
            target.ReadTestData();

            for (int testCount = 0; testCount < testQueryCriteria.GetLength(0); testCount++)
            {
                bool dateRangeSpecified = (bool)testQueryCriteria[testCount, 0];
                DateTime dateStart = (DateTime)testQueryCriteria[testCount, 1];
                DateTime dateEnd = (DateTime)testQueryCriteria[testCount, 2];
                if (dateEnd < DateTime.MaxValue)
                {
                    dateEnd = dateEnd.AddDays(1);
                }

                bool temperatureRangeSpecified = (bool)testQueryCriteria[testCount, 3];
                short temperatureStart = (short)testQueryCriteria[testCount, 4];
                short temperatureEnd = (short)testQueryCriteria[testCount, 5];
                bool stressRangeSpecified = (bool)testQueryCriteria[testCount, 6];
                short appliedStressStart = (short)testQueryCriteria[testCount, 7];
                short appliedStressEnd = (short)testQueryCriteria[testCount, 8];
                bool deflectionRangeSpecified = (bool)testQueryCriteria[testCount, 9];
                short deflectionStart = (short)testQueryCriteria[testCount, 10];
                short deflectionEnd = (short)testQueryCriteria[testCount, 11];
                int expectedCount = (int)testQueryCriteria[testCount, 12];

                OrderByKey_Accessor orderby = new OrderByKey_Accessor();
                orderby.value__ = 0;
                bool limitRows = false;
                int rowCount = 0;

                IEnumerable<TestResult> actual = target.CreateQuery(dateRangeSpecified, dateStart, dateEnd,
                    temperatureRangeSpecified, temperatureStart, temperatureEnd,
                    stressRangeSpecified, appliedStressStart, appliedStressEnd,
                    deflectionRangeSpecified, deflectionStart, deflectionEnd,
                    orderby, limitRows, rowCount);
                int actualCount = actual.Count();
                Assert.AreEqual(expectedCount, actualCount);

                string actualFormattedResults = target.FormatResults(actual);
                int actualFormattedResultsLength = actualFormattedResults.Length;
                int expectedFormattedResultsLength = (int)testQueryCriteria[testCount, 13];
                Assert.AreEqual(expectedFormattedResultsLength, actualFormattedResultsLength);
            }
        }
    }
}
