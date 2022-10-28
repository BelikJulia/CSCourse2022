using GenerateGraph;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.IO;

namespace TestGenerateGraph
{     
    /// <summary>
    ///This is a test class for GraphWindowTest and is intended
    ///to contain all GraphWindowTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GraphWindowTest
    {
        private const string fileName = @"E:\Labfiles\Lab 15\StressData.xlsx";
        private Excel.Application excelApp = null;
        private Excel.Workbook excelWB = null;
        private Excel.Worksheet excelWS = null;

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
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
        

        [TestInitialize()]
        public void GraphTestInitialize()
        {
            excelApp = new Excel.Application();
            excelApp.Visible = true;
            excelApp.AlertBeforeOverwriting = false;
            excelApp.DisplayAlerts = false;
            excelWB = excelApp.Workbooks.Add();
            excelWS = excelWB.ActiveSheet;
        }


        [TestCleanup()]
        public void GraphTestCleanup()
        {
            excelApp.Quit();
        }
        
        #endregion


        /// <summary>
        ///A test for transferDataToExcelSheet
        ///</summary>
        [TestMethod()]
        [DeploymentItem("GenerateGraph.exe")]
        public void transferDataToExcelSheetTest()
        {
            List<StressData> graphData = new List<StressData>();
            StressData data = new StressData();

            GraphWindow_Accessor target = new GraphWindow_Accessor();
            target.populateFromFile(data, @"E:\Labfiles\Lab 15\298K.txt");
            graphData.Add(data);
            data = new StressData();
            target.populateFromFile(data, @"E:\Labfiles\Lab 15\318K.txt");
            graphData.Add(data);
            data = new StressData();
            target.populateFromFile(data, @"E:\Labfiles\Lab 15\338K.txt");
            graphData.Add(data);

            Excel.Range excelRange = null; 
            bool expected = true; 
            bool actual;
            actual = target.transferDataToExcelSheet(excelWS, out excelRange, graphData);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(excelRange.Column, 1);
            Assert.AreEqual(excelRange.Columns.Count, 4);
            Assert.AreEqual(excelRange.Row, 1);
            Assert.AreEqual(excelRange.Rows.Count, 16);            
         }

        /// <summary>
        ///A test for generateExcelChart
        ///</summary>
        [TestMethod()]
        [DeploymentItem("GenerateGraph.exe")]
        public void generateExcelChartTest()
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            Excel.Range excelRange = excelWS.Cells;
            GraphWindow_Accessor.generateExcelChart(fileName, excelWB, excelRange);
            Assert.AreEqual(excelWB.Charts.Count, 1);
            Assert.IsTrue(System.IO.File.Exists(fileName));
        } 
    }
}
