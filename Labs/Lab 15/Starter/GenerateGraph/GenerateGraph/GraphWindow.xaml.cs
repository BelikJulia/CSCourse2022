using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// System.IO contains the StreamReader class, used to read data from CSV files
using System.IO;

// Microsoft.Win32 contains the Open File and Save File common dialogs 
// used to prompt the user for the names of the CSV files and the Excel worksheet
using Microsoft.Win32;

//Add the Microsoft.Office.Interop.Excel namespace
using Excel = Microsoft.Office.Interop.Excel;


namespace GenerateGraph
{
    /// <summary>
    /// WPF application to read stress analysis data from CSV files
    /// and generate an Excel chart
    /// </summary>
    public partial class GraphWindow : Window
    {
        /// <summary>
        /// graphData contains the stress analysis data
        /// <para>
        /// This variable is a List of StressData objects. Each StressData object holds
        /// the stress analysis results for single temperature
        /// </para>
        /// </summary>
        private List<StressData> graphData = null;
        
        /// <summary>
        /// Initialize the WPF window and the graphData variable
        /// </summary>
        public GraphWindow()
        {
            InitializeComponent();
            this.graphData = new List<StressData>();            
        }

        /// <summary>
        /// Read a CSV data containing stress analysis data and populate a StressData object with this information
        /// <para>
        /// The data is held in the following format:
        /// <code>
        /// 298
        /// 100,15
        /// 200,35
        /// 300,55
        /// ...
        /// 1500,550
        /// </code>
        /// </para>
        /// <para>
        /// The first line specifies the temperature used for the test (in Kelvin).
        /// </para>
        /// <para>
        /// Subsequent lines contain pairs of values: the applied stress (in kN), and the deflection resulting from this stress (in mm).
        /// </para>
        /// <para>
        /// The applied stress is specified in 100 kN increments, from 100kN to 1500 kN. 
        /// The deflection data for a specified stress may be absent (usually if the applied stress caused a complete failure).
        /// </para>
        /// </summary>
        /// <param name="stressData">
        /// The StressData object to populate. This object must have been created prior to calling the method.
        /// </param>
        /// <param name="fileName">
        /// The name of the file containing the CSV data
        /// </param>
        /// <returns>
        /// True if the StressData object was populated successfully, false otherwise
        /// </returns>
        private bool populateFromFile(StressData stressData, string fileName)
        {
            try
            {
                // Open the CSV file for reading 
                using (StreamReader dataFile = new StreamReader(fileName))
                {
                    // Read the temperature held in the first line of the file and save it in the StressData object
                    stressData.Temperature = short.Parse(dataFile.ReadLine());

                    // Initialize the List of stress/deflection pairs in the StressData object
                    stressData.Data = new Dictionary<short, short?>();

                    char[] seperators = new char[] { ',' };

                    // Read each line until the end of the file
                    while (!dataFile.EndOfStream)
                    {
                        string nextStressDataLine = dataFile.ReadLine();

                        // Parse the data. Split it using the comma as the seperator
                        string[] nextStressDataValues = nextStressDataLine.Split(seperators);
                        short appliedStress;

                        // Parse the applied stress value. This should be a short. 
                        // If it is not, then skip this line
                        if (short.TryParse(nextStressDataValues[0], out appliedStress))
                        {
                            // Parse the defelction value. This should also be a short.
                            // If it is, then add the applied stress/deflection pair to the List in the StressData object
                            // Otherwise add the applied stress but specify a null value for the deflection
                            short deflection;
                            if (short.TryParse(nextStressDataValues[1], out deflection))
                            {
                                stressData.Data.Add(appliedStress, deflection);
                            }
                            else
                            {
                                stressData.Data.Add(appliedStress, null);
                            }
                        }                        
                    }
                    // On success, return true
                    return true;
                }
            }

            catch(Exception e)
            {
                // If an exception occurs, alert the user and return false
                MessageBox.Show(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Event-handling method for the Click event of the getData button on the WPF form.
        /// <para>
        /// This method gets the name of a file containing CSV data, creates a StressData object, 
        /// and then calls the <c>populateFromFile</c> method to read the data and populate it.
        /// </para>
        /// <seealso cref="populateFromFile"/>
        /// <seealso cref="displayData" />
        /// </summary>
        private void getData_Click(object sender, RoutedEventArgs e)
        {
            // Prompt the user for the name of the file containing the CSV data by using the Open File common dialog.
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.DefaultExt = "txt";
            openDialog.Multiselect = false;
            openDialog.InitialDirectory = @"E:\Labfiles\Lab 15\";
            openDialog.ValidateNames = true;
            openDialog.Title = "Graph Data";

            // If the user specifies a valid filename, create a StressData object and populate it
            // with data in the file.
            // Display the data when it has been read in.
            if (openDialog.ShowDialog().Value)
            {
                StressData stressData = new StressData();
                if (populateFromFile(stressData, openDialog.FileName))
                {
                    this.graphData.Add(stressData);
                    displayData(stressData);
                }
            }
        }

        /// <summary>
        /// Display the data in a StressData object by adding it to a TreeView control on the WPF form.
        /// </summary>
        /// <param name="stressData">
        /// The StressData object containing the data to add to the TreeView control
        /// </param>
        private void displayData(StressData stressData)
        {
            // Create a new TreeViewItem object and fill it with the data from the StressData object
            TreeViewItem displayItem = new TreeViewItem();
            displayItem.Header = String.Format("Temperature: {0}K", stressData.Temperature);
            foreach (var item in stressData.Data)
            {
                displayItem.Items.Add(String.Format("Stress: {0}kN\t\tDeflection: {1}mm", item.Key, item.Value ?? -1));
            }

            // Add the TreeViewItem object to the TreeView control and display it on the WPF form
            this.dataDisplay.Items.Add(displayItem);
        }

        /// <summary>
        /// Event-handling method for the Click event of the generateGraph button on the WPF form.
        /// <para>
        /// This method copies the data in the list of StressData objects in the graphData variable
        /// to a an Excel spreadsheet, and then generates an Excel chart.
        /// </para>
        /// The spreadsheet and chart is saved in a file with the name specified by the user.
        /// <seealso cref="transferDataToExcelSheet"/>
        /// </summary>
        private void generateGraph_Click(object sender, RoutedEventArgs e)
        {
            // Verify that the graphData variable actually contains some data.
            // Do not invoke Excel or attempt to generate a chart if there is no data.
            if (graphData.Count > 0)
            {
                Excel.Application excelApp = null;

                try
                {
                    // Prompt the user for the name of the Excel file to create by using
                    // the Save File common dialog
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.DefaultExt = "xlsx";
                    saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx;*.xls";
                    saveDialog.InitialDirectory = @"E:\Labfiles\Lab 15\";
                    saveDialog.OverwritePrompt = true;
                    saveDialog.FileName = "StressData";
                    saveDialog.ValidateNames = true;
                    saveDialog.Title = "Graph Data";

                    if (saveDialog.ShowDialog().Value)
                    {
                        // If the user specifies a valid file name, start Excel 
                        // and create a new workbook and worksheet to hold the data
                        excelApp = new Excel.Application();
                        excelApp.Visible = true;
                        excelApp.AlertBeforeOverwriting = false;
                        excelApp.DisplayAlerts = false;
                        Excel.Workbook excelWB = excelApp.Workbooks.Add();
                        Excel.Worksheet excelWS = excelWB.ActiveSheet;
                        // Copy the data from the graphData variable to the new worksheet and generate a graph
                        // The dataRange variable specifies the cells on the worksheet that hold the data
                        Excel.Range dataRange = null;
                        if (transferDataToExcelSheet(excelWS, out dataRange, this.graphData))
                            generateExcelChart(saveDialog.FileName, excelWB, dataRange);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    // Close Excel and release any resources
                    if (excelApp != null)
                        excelApp.Quit();
                }
            }
            else
            {
                // If the graphData variable is empty, display a message to the user
                MessageBox.Show("No data loaded");
            }
        }

        /// <summary>
        /// Generate a chart from the data in an Excel worksheet, add it to the workbook, and save it.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file to save the Excel workbook.
        /// </param>
        /// <param name="excelWB">
        /// The Excel workbook to add the chart to.
        /// </param>
        /// <param name="dataRange">
        /// The range specifying the data to use for the chart.
        /// </param>
        private static void generateExcelChart(string fileName, Excel.Workbook excelWB, Excel.Range dataRange)
        {
            // Generate a line graph based on the data in the dataRange range.
            Excel.Chart excelChart = excelWB.Charts.Add();
            excelChart.ChartWizard(Title: "Applied Stress (kN) versus Deflection (mm)",
                                   Source: dataRange, Gallery: Excel.XlChartType.xlLine,
                                   PlotBy: Excel.XlRowCol.xlColumns, CategoryLabels: 1, SeriesLabels: 1,
                                   ValueTitle: "Deflection", CategoryTitle: "Applied Stress");
            // Save the Excel workbook
            excelWB.SaveAs(Filename: fileName);
        }

        /// <summary>
        /// Copy the data from the graphData variable to an Excel worksheet.
        /// </summary>
        /// <param name="excelWS">
        /// The Excel worksheet to hold the data.
        /// </param>
        /// <param name="dataRange">
        /// The range in the Excel worksheet that holds the data. This is an <c>out</c> parameter.
        /// </param>
        /// <param name="excelData">
        /// The data to copy to the worksheet
        /// </param>
        /// <returns>
        /// True if the data is copied sucessfully, false otherwise.
        /// </returns>
        private bool transferDataToExcelSheet(Excel.Worksheet excelWS, out Excel.Range dataRange, List<StressData> excelData)
        {
            try
            {
                // Copy the data for the applied stresses to the first column in the worksheet. 
                // This should be a list of values: 100, 200, 300, ..., 1500
                // Each set of data in the list in the graphData object uses the same set of stresses.
                int rowNum = 1;
                int colNum = 1;
                excelWS.Cells[rowNum, colNum] = "Applied Stress";

                foreach (short appliedStress in excelData[0].Data.Keys)
                {
                    rowNum++;
                    excelWS.Cells[rowNum, colNum] = appliedStress;
                }
                // Copy the deflection data for each set of test results to a new column in the worksheet
                foreach (StressData deflectionData in excelData)
                {

                    // Give each column a header that specifies the temperature
                    colNum++;
                    rowNum = 1;
                    excelWS.Cells[rowNum, colNum] = String.Format("{0}K", deflectionData.Temperature);

                    // Copy the delection data to this column in the worksheet
                    foreach (short? deflection in deflectionData.Data.Values)
                    {
                        // Only copy the deflection value if it is not null
                        if (deflection != null)
                        {
                            rowNum++;
                            excelWS.Cells[rowNum, colNum] = deflection;
                        }
                    }
                }

                // Specify the range of cells in the spreadsheet containing the data in the dataRange variable
                dataRange = excelWS.UsedRange;
                // Return true to indicate that the data has been successfully copied
                return true;
            }
            catch (Exception ex)
            {
                // If an exception occurs display a message, set dataRange to null, and return false
                MessageBox.Show(ex.Message);
                dataRange = null;
                return false;
            }
        }
     }
}
