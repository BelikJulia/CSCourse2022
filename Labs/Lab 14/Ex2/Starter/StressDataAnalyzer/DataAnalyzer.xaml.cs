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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;

// Namespaces containing the Tree and TestResult types
using BinaryTree;
using StressTestResult;

// Namespace containing counters and other performance diagnostics
using System.Diagnostics;

// Namespace containing the LINQ Expression types
using Expressions = System.Linq.Expressions;

// Namespace containing types used for reflecting data types
using System.Reflection;

namespace StressDataAnalyzer
{
    /// <summary>
    /// Enumeration that specifies the ways in which the user can sort data
    /// </summary>
    enum OrderByKey { ByDate, ByTemperature, ByAppliedStress, ByDeflection, None }

    /// <summary>
    /// WPF application to enable a user to query stress data and display the results.
    /// <para>
    /// The data is held in a file and read into a binary tree structure holding TestResult structs.
    /// </para>
    /// <para>
    /// The user specifies the fields to display, query criteria, and sort keys by using the form
    /// and then clicks Display.
    /// </para>
    /// <para>
    /// The application displays the results in a TextBox control on the form.
    /// </para>
    /// </summary>
    public partial class DataAnalyzer : Window
    {
        /// <summary>
        /// stressDataFileName holds the name of the file containing the stress test data.
        /// </summary>
        private const string stressDataFilename = @"E:\Labfiles\Lab 14\StressData.dat";

        /// <summary>
        /// stressData is the binary tree that holds the data read from the file. 
        /// The elements are instances of the TestResult struct.
        /// </summary>
        private Tree<TestResult> stressData = null;

        public DataAnalyzer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event-handling method for the Loaded event of the WPF window.
        /// <para>
        /// This method calls readTestData to read in the test data and populate the binary tree with the results.
        /// </para>
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Read the test data and populate the binary tree
            // Use a BackgroundWorker to avoid tying up the user interface
            BackgroundWorker workerThread = new BackgroundWorker();
            workerThread.WorkerReportsProgress = false;
            workerThread.WorkerSupportsCancellation = false;

            workerThread.DoWork += (o, args) =>
            {
                this.ReadTestData();
            };

            // When the BackgroundWorker has completed reading the test data
            // set the status bar to "Ready" and enable the Display button
            workerThread.RunWorkerCompleted += (o, args) =>
            {
                this.displayResults.IsEnabled = true;
                this.statusMessage.Content = "Ready";
            };

            // Start the BackgroundWorker and set the status bar to "Reading test data ..."
            workerThread.RunWorkerAsync();
            this.statusMessage.Content = "Reading test data ...";
        }

        /// <summary>
        /// Method that reads the test data from the file specified by the stressDataFileName string
        /// and creates the stressData binary tree using this data.
        /// </summary>
        private void ReadTestData()
        {
            try
            {
                // Open a stream over the file holding the test data
                using (FileStream readStream = File.Open(stressDataFilename, FileMode.Open))
                {
                    // The data is serialized as TestResult instances.
                    // Use a BinaryFormatter to read the stream and deserialize the data.
                    BinaryFormatter formatter = new BinaryFormatter();
                    TestResult initialNode = (TestResult)formatter.Deserialize(readStream);

                    // Create the binary tree and use the first item retrieved as the root node.
                    // (Note: The tree will likely be unbalanced, as it is probable that most nodes will have
                    //  a value that is greater than or equal to the value in this root node - this is due
                    //  to the way in which the test results are generated and the fact that the TestResult
                    //  class uses the deflection as the discriminator when comparing instances.)
                    stressData = new Tree<TestResult>(initialNode);

                    // Read the TestResult instances from the rest of the file
                    // and add them into the binary tree
                    while (readStream.Position < readStream.Length)
                    {
                        TestResult data = (TestResult)formatter.Deserialize(readStream);
                        stressData.Insert(data);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Event-handling methods for enabling/disabling query criteria and row limiting
        private void SpecifyDateRange_Checked(object sender, RoutedEventArgs e)
        {
            this.startDate.IsEnabled = true;
            this.endDate.IsEnabled = true;
        }

        private void SpecifyDateRange_Unchecked(object sender, RoutedEventArgs e)
        {
            this.startDate.IsEnabled = false;
            this.endDate.IsEnabled = false;
        }

        private void SpecifyTemperatureRange_Checked(object sender, RoutedEventArgs e)
        {
            this.fromTemperature.IsEnabled = true;
            this.toTemperature.IsEnabled = true;
        }

        private void SpecifyTemperatureRange_Unchecked(object sender, RoutedEventArgs e)
        {
            this.fromTemperature.IsEnabled = false;
            this.toTemperature.IsEnabled = false;
        }

        private void SpecifyAppliedStressRange_Checked(object sender, RoutedEventArgs e)
        {
            this.fromAppliedStress.IsEnabled = true;
            this.toAppliedStress.IsEnabled = true;
        }

        private void SpecifyAppliedStressRange_Unchecked(object sender, RoutedEventArgs e)
        {
            this.fromAppliedStress.IsEnabled = false;
            this.toAppliedStress.IsEnabled = false;
        }

        private void SpecifyDeflectionRange_Checked(object sender, RoutedEventArgs e)
        {
            this.fromDeflection.IsEnabled = true;
            this.toDeflection.IsEnabled = true;
        }

        private void SpecifyDeflectionRange_Unchecked(object sender, RoutedEventArgs e)
        {
            this.fromDeflection.IsEnabled = false;
            this.toDeflection.IsEnabled = false;
        }

        private void Limit_Checked(object sender, RoutedEventArgs e)
        {
            this.numRows.IsEnabled = true;
        }

        private void Limit_Unchecked(object sender, RoutedEventArgs e)
        {
            this.numRows.IsEnabled = false;
        }

        #endregion

        /// <summary>
        /// Event-handling method for the Click event of the Display button.
        /// <para>
        /// This method retrieves the field selection, the query criteria, and the sort sequence
        /// entered by the user on the form and then calls the CreateQuery method to generate an 
        /// enumerable result set.
        /// </para>
        /// The results are formatted by using the FormatResults method, 
        /// and are then displayed in the results TextBox control on the form.
        /// </summary>
        private void DisplayResults_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Capture the criteria for the start and end dates. These require different handling from the other criteria
                // Use DateTime.MinValue and DateTime.MaxValue as default values
                DateTime dateStart = String.IsNullOrWhiteSpace(this.startDate.Text) ? DateTime.MinValue : DateTime.Parse(this.startDate.Text);
                DateTime dateEnd = String.IsNullOrWhiteSpace(this.endDate.Text) ? DateTime.MaxValue : DateTime.Parse(this.endDate.Text); ;

                // The date and times in the test data include a time of day, 
                // whereas the dates selected by the user will have the time of day set to midnight.
                // Consequently, you need to add 1 day to the end date specified by the user
                // to avoid losing test results generated on that date.
                if (dateEnd < DateTime.MaxValue)
                {
                    dateEnd = dateEnd.AddDays(1);
                }

                // Capture the temperature range criteria
                short temperatureStart = (short)this.fromTemperature.Value;
                short temperatureEnd = (short)this.toTemperature.Value;

                // Capture the applied stress criteria
                short appliedStressStart = (short)this.fromAppliedStress.Value;
                short appliedStressEnd = (short)this.toAppliedStress.Value;

                // Capture the deflection criteria
                short deflectionStart = (short)this.fromDeflection.Value;
                short deflectionEnd = (short)this.toDeflection.Value;

                // Determine how (and whether) the user wants to sort data
                OrderByKey orderByKey = OrderByKey.None;

                if (this.orderByDate.IsChecked.Value)
                {
                    orderByKey = OrderByKey.ByDate;
                }

                if (this.orderByTemperature.IsChecked.Value)
                {
                    orderByKey = OrderByKey.ByTemperature;
                }

                if (this.orderByAppliedStress.IsChecked.Value)
                {
                    orderByKey = OrderByKey.ByAppliedStress;
                }

                if (this.orderByDeflection.IsChecked.Value)
                {
                    orderByKey = OrderByKey.ByDeflection;
                }

                // Generate an enumerable result set using these criteria
                IEnumerable<TestResult> query = CreateQuery(this.specifyDateRange.IsChecked.Value, dateStart, dateEnd,
                                                            this.specifyTemperatureRange.IsChecked.Value, temperatureStart, temperatureEnd,
                                                            this.specifyAppliedStressRange.IsChecked.Value, appliedStressStart, appliedStressEnd,
                                                            this.specifyDeflectionRange.IsChecked.Value, deflectionStart, deflectionEnd,
                                                            orderByKey, this.limit.IsChecked.Value, (int)this.numRows.Value);

                // Determine how long the quety actually takes to run -
                // Calling the Count() method retrieves all rows
                Stopwatch timer = Stopwatch.StartNew();
                int rowCount = query.Count();
                long timeTaken = timer.ElapsedMilliseconds;
                queryTime.Content = String.Format("Time (ms): {0}", timeTaken);

                // Format the results into a string
                // This might take some time, so use a BackgroundWorker to avoid tying up the user interface
                BackgroundWorker workerThread = new BackgroundWorker();
                workerThread = new BackgroundWorker();
                workerThread.WorkerReportsProgress = false;
                workerThread.WorkerSupportsCancellation = false;

                // Return the formatted string as the result of the background operation
                workerThread.DoWork += (o, args) =>
                {
                    args.Result = FormatResults(query);
                };

                // When the BackgroundWorker has completed reading the test data
                // display the results, set the status bar to "Ready" 
                // and enable the Display button
                workerThread.RunWorkerCompleted += (o, args) =>
                {
                    this.results.Text = args.Result as string;
                    this.displayResults.IsEnabled = true;
                    this.statusMessage.Content = "Ready";
                };

                // Start the BackgroundWorker, disable the Display button,
                // and set the status bar to "Fetching results ..."
                workerThread.RunWorkerAsync();
                this.displayResults.IsEnabled = false;
                this.statusMessage.Content = "Fetching results ...";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Method that generates an enumerable collection of TestResult items from the stressData
        /// binary tree, based on the criteria and sort key specified by the user. All data fetched will fall within
        /// the range specified by these criteria.
        /// </summary>
        /// <param name="dateRangeSpecified">
        /// A boolean that indicates whether the specified criteria for the test date data.
        /// </param>
        /// <param name="dateStart">
        /// The start date criterion.
        /// </param>
        /// <param name="dateEnd">
        /// The end date criterion.
        /// </param>
        /// <param name="temperatureRangeSpecified">
        /// A boolean that indicates whether the specified criteria for the temperature data.
        /// </param>
        /// <param name="temperatureStart">
        /// The lower temperature criterion,
        /// </param>
        /// <param name="temperatureEnd">
        /// The upper temperature criterion.
        /// </param>
        /// <param name="appliesStressRangeSpecified">
        /// A boolean that indicates whether the specified criteria for the applied stress data.
        /// </param>
        /// <param name="appliedStressStart">
        /// The lower applied stress criterion.
        /// </param>
        /// <param name="appliedStressEnd">
        /// The upper applied stress criterion.
        /// </param>
        /// <param name="deflectionRangeSpecified">
        /// A boolean that indicates whether the specified criteria for the deflection data.
        /// </param>
        /// <param name="deflectionStart">
        /// The lower deflection criterion.
        /// </param>
        /// <param name="deflectionEnd">
        /// The upper deflection criterion.
        /// </param>
        /// <param name="orderByKey">
        /// </param>
        /// The sort key. Data will be retrieved in ascending order of this key.
        /// <returns>
        /// An IEnumerable&lt;TestResult&gt; object that can be used to iterate through the results.
        /// </returns>
        private IEnumerable<TestResult> CreateQuery(bool dateRangeSpecified, DateTime dateStart, DateTime dateEnd,
                    bool temperatureRangeSpecified, short temperatureStart, short temperatureEnd,
                    bool appliedStressRangeSpecified, short appliedStressStart, short appliedStressEnd,
                    bool deflectionRangeSpecified, short deflectionStart, short deflectionEnd,
                    OrderByKey orderByKey, bool limitRows, int numRows)
        {
            // Examine the CreateQuery method

            // Build the lambda expression that encapsulates the query criteria specified by the user
            // This can be null if the user specified no criteria
            Expressions.Expression<Func<TestResult, bool>>
                getCriteria = BuildLambdaExpressionForQueryCriteria(dateRangeSpecified, dateStart, dateEnd,
                                          temperatureRangeSpecified, temperatureStart, temperatureEnd,
                                          appliedStressRangeSpecified, appliedStressStart, appliedStressEnd,
                                          deflectionRangeSpecified, deflectionStart, deflectionEnd);

            // Build the lambda expression that defines the sort key specified by the user
            // This can be null if the user elected to use the default sort key implemented by the TestResult type
            Expressions.Expression<Func<TestResult, ValueType>>
                getOrderBy = BuildLambdaExpressionForOrderBy(orderByKey);

            // Construct an enumerable query object
            // Only filter and sort data if necessary
            IEnumerable<TestResult> query = stressData;

            if (getCriteria != null)
            {
                query = query.Where(getCriteria.Compile());
            }

            if (getOrderBy != null)
            {
                query = query.OrderBy(getOrderBy.Compile());
            }

            // If the user specified to limit the number of rows returned
            // then only retrieve the first numRows rows
            if (limitRows)
            {
                query = query.Take(numRows);
            }

            return query;
        }

        /// <summary>
        /// Fetch the data defined by the LINQ query specified as the parameter 
        /// and format the results as a string.
        /// </summary>
        /// <param name="query">
        /// The IEnumerable&lt;TestResult&gt;
        /// </param>
        /// <returns>
        /// A formatted string that contains the data fetched by the query.
        /// </returns>
        private string FormatResults(IEnumerable<TestResult> query)
        {
            // Use a StringBuilder object to construct the formatted string
            StringBuilder builder = new StringBuilder();

            // Add a heading and indicate the number of matching results retrieved
            builder.Append(String.Format("Stress Test Results. Number of matching items: {0}\n\n", query.Count()));

            // Add column headings
            builder.Append("Test Date\t\tTemperature\tApplied Stress\tDeflection\n");

            // Iterate through the results and format each item found
            foreach (var item in query)
            {
                builder.Append(String.Format("{0:d}\t\t{1}\t\t{2}\t\t{3}\n",
                    item.TestDate, item.Temperature, item.AppliedStress, item.Deflection));
            }

            // Return the string constructed by using the StringBuilder object
            return builder.ToString();
        }

        /// <summary>
        /// Method that dynamically generates a lambda expression that matches the query criteria specified by the user.
        /// </summary>
        /// <param name="dateRangeSpecified">
        /// A boolean. It is true if the user specified a date range for filtering data.
        /// </param>
        /// <param name="startDate">
        /// The lower value of the date range, if specified.
        /// </param>
        /// <param name="endDate">
        /// The upper value of the date range, if specified.
        /// </param>
        /// <param name="temperatureRangeSpecified">
        /// A boolean. It is true if the user specified a temperature range for filtering data.</param>
        /// <param name="fromTemperature">
        /// The lower value of the temperature range, if specified.
        /// </param>
        /// <param name="toTemperature">
        /// The upper value of the temperature range, if specified.
        /// </param>
        /// <param name="appliedStressRangeSpecified">
        /// A boolean. It is true if the user specified an applied stress range for filtering data.</param>
        /// <param name="fromStressRange">
        /// The lower value of the applied stress range, if specified.
        /// </param>
        /// <param name="toStressRange">
        /// The upper value of the applied stress range, if specified.
        /// </param>
        /// <param name="deflectionRangeSpecified">
        /// A boolean. It is true if the user specified a deflection range for filtering data.
        /// </param>
        /// <param name="fromDeflection">
        /// The lower value of the deflection range, if specified.
        /// </param>
        /// <param name="toDeflection">
        /// The upper value of the deflection range, if specified.
        /// </param>
        /// <returns>
        /// An Expression that defines a lambda expression that filters data using the criteria specified by the user,
        /// or null if the user did not specify any query criteria.
        /// </returns>
        private Expressions.Expression<Func<TestResult, bool>>
            BuildLambdaExpressionForQueryCriteria(bool dateRangeSpecified, DateTime startDate, DateTime endDate,
                                                  bool temperatureRangeSpecified, short fromTemperature, short toTemperature,
                                                  bool appliedStressRangeSpecified, short fromStressRange, short toStressRange,
                                                  bool deflectionRangeSpecified, short fromDeflection, short toDeflection)
        {
            // Define an Expression object to populate
            Expressions.Expression<Func<TestResult, bool>> lambda = null;

            // Verify that the user actually specified some criteria
            if (dateRangeSpecified || temperatureRangeSpecified || appliedStressRangeSpecified || deflectionRangeSpecified)
            {
                // Complete the BuildLambdaExpressionForQueryCriteria method.
                // Create the expression that defines the parameter for the 
                // lambda expression.
                // The type is TestResult, and the lambda expression refers to
                // it with the name "item".
                Type testResultType = typeof(TestResult);
                Expressions.ParameterExpression itemBeingQueried = Expressions.Expression.Parameter(testResultType, "item");
                // Create expressions for each of the possible conditions.
                Expressions.BinaryExpression dateCondition = null;
                Expressions.BinaryExpression temperatureCondition = null;
                Expressions.BinaryExpression appliedStressCondition = null;
                Expressions.BinaryExpression deflectionCondition = null;
                // Build Boolean expressions for each of the possible criteria 
                // that the user specifies.
                // These method calls may return null if the user did not 
                // specify criteria for a property.
                dateCondition = BuildDateExpressionBody(dateRangeSpecified, startDate, endDate, testResultType, itemBeingQueried);
                temperatureCondition = BuildNumericExpressionBody
                    (temperatureRangeSpecified, fromTemperature, toTemperature,
                    testResultType, "Temperature", itemBeingQueried);
                appliedStressCondition = BuildNumericExpressionBody
                    (appliedStressRangeSpecified, fromStressRange, toStressRange,
                    testResultType, "AppliedStress", itemBeingQueried);
                deflectionCondition = BuildNumericExpressionBody
                    (deflectionRangeSpecified, fromDeflection, toDeflection,
                    testResultType, "Deflection", itemBeingQueried);
                // Combine the Boolean expressions together into a single body.
                Expressions.Expression body = BuildLambdaExpressionBody
                    (dateCondition, temperatureCondition,
                    appliedStressCondition, deflectionCondition);
                // Build the lambda expression using the parameter and the 
                // body expressions.
                lambda = Expressions.Expression.Lambda<Func<TestResult, bool>> (body, itemBeingQueried);
            }

            // Return the lambda expression. If the user did not specify any criteria this value will be null.
            return lambda;
        }

        #region Support methods used to build the dynamic lambda expression for specifying query criteria

        /// <summary>
        /// Method that builds the boolean expression that evaluates criteria specified for the date range.
        /// </summary>
        /// <param name="dateRangeSpecified">
        /// Boolean value that indicates whether the user specified a date range.
        /// </param>
        /// <param name="startDate">
        /// The start date specified by the user.
        /// </param>
        /// <param name="endDate">
        /// The end date specified by the user.
        /// </param>
        /// <param name="testResultType">
        /// The type of the TestResult structure holding the TestDate property
        /// </param>
        /// <param name="itemBeingQueried">
        /// The parameter passed in to the lambda expression containing the item in the TestResult collection being examined.
        /// </param>
        /// <returns>
        /// A boolean Expression object, or null if the user did not specify a date range.
        /// </returns>
        private Expressions.BinaryExpression BuildDateExpressionBody(bool dateRangeSpecified, DateTime startDate, DateTime endDate, Type testResultType, Expressions.ParameterExpression itemBeingQueried)
        {
            // Define an Expression object to populate.
            Expressions.BinaryExpression dateCondition = null;

            // If the user has specified a date range, generate the expression:
            //
            //     item.TestDate >= startDate && item.TestDate <= endDate
            //
            if (dateRangeSpecified)
            {
                // Complete the BuildDateExpressionBody method.
                // Generate the expression:
                //
                //    item.TestDate >= startDate
                //
                MemberInfo testDateProperty = testResultType.GetProperty("TestDate");
                Expressions.MemberExpression testDateMember = Expressions.Expression.MakeMemberAccess(itemBeingQueried, testDateProperty);
                Expressions.ConstantExpression lowerDate = Expressions.Expression.Constant(startDate);
                Expressions.BinaryExpression lowerDateCondition = Expressions.Expression.GreaterThanOrEqual(testDateMember, lowerDate);
                // Generate the expression:
                //
                //    item.Testdate <= endDate
                //
                Expressions.ConstantExpression upperDate = Expressions.Expression.Constant(endDate);
                Expressions.BinaryExpression upperDateCondition = Expressions.Expression.LessThanOrEqual(testDateMember, upperDate);
                // Combine the expressions with the && operator.
                dateCondition = Expressions.Expression.AndAlso(lowerDateCondition, upperDateCondition);
            }

            // Return the expression.
            return dateCondition;
        }

        /// <summary>
        /// Method that builds the boolean expression that evaluates criteria specified for the numeric properties 
        /// (temperature, applied stress, and deflection).
        /// </summary>
        /// <param name="dateRangeSpecified">
        /// Boolean value that indicates whether the user specified a range for the property.
        /// </param>
        /// <param name="lowerRange">
        /// The lower limit specified by the user.
        /// </param>
        /// <param name="upperRange">
        /// The upper limit specified by the user.
        /// </param>
        /// <param name="testResultType">
        /// The type of the TestResult structure holding the property being examined
        /// </param>
        /// <param name="propertyName">
        /// The name of the property in the TestResult structure the criteria query.
        /// </param>
        /// <param name="itemBeingQueried">
        /// The parameter passed in to the lambda expression containing the item in the TestResult collection being examined.
        /// </param>
        /// <returns>
        /// A boolean Expression object, or null if the user did not specify a date range.
        /// </returns>
        private Expressions.BinaryExpression BuildNumericExpressionBody(bool rangeSpecified, short lowerRange, short upperRange, Type testResultType, string propertyName, Expressions.ParameterExpression itemBeingQueried)
        {
            // Define an Expression object to populate.
            Expressions.BinaryExpression booleanCondition = null;

            // If the user has specified a range, generate the expression:
            //
            //     item.<Property> >= lowerRange && item.<Property> <= upperRange
            //
            if (rangeSpecified)
            {
                // Complete the BuildNumericExpressionBody method.
                // Generate the expression:
                //
                //    item.<Property> >= lowerRange
                //
                MemberInfo testProperty = testResultType.GetProperty(propertyName);
                Expressions.MemberExpression testMember = Expressions.Expression.MakeMemberAccess(itemBeingQueried, testProperty);
                Expressions.ConstantExpression lowerValue = Expressions.Expression.Constant(lowerRange);
                Expressions.BinaryExpression lowerValueCondition = Expressions.Expression.GreaterThanOrEqual(testMember, lowerValue);
                // Generate the expression:
                //
                //    item.<Property> <= upperRange
                //
                Expressions.ConstantExpression upperValue = Expressions.Expression.Constant(upperRange);
                Expressions.BinaryExpression upperValueCondition = Expressions.Expression.LessThanOrEqual(testMember, upperValue);
                // Combine the expressions with the && operator.
                booleanCondition = Expressions.Expression.AndAlso(lowerValueCondition, upperValueCondition);
            }

            // Return the expression.
            return booleanCondition;
        }


        /// <summary>
        /// Combine the boolean expressions defined by the parameters into a single boolean expression.
        /// If all the expressions are null, return an expression that evaluates to True.
        /// </summary>
        /// <param name="dateCondition">
        /// The date boolean expression (item.TestDate &ge; date1 && item.TestDate &le; date2)
        /// This may be null.
        /// </param>
        /// <param name="temperatureCondition"></param>
        /// The temperature boolean expression (item.Temperature &ge; temp1 && item.Temperature &le; temp2)
        /// This may be null.
        /// <param name="appliedStressCondition">
        /// The applied stress boolean expression (item.AppliedStress &ge; stress1 && item.AppliedStress &le; stress2)
        /// This may be null.
        /// </param>
        /// <param name="deflectionCondition">
        /// The deflection boolean expression (item.Deflection &ge; def1 && item.Deflection &le; def2)
        /// This may be null.
        /// </param>
        /// <returns></returns>
        private Expressions.Expression BuildLambdaExpressionBody(Expressions.BinaryExpression dateCondition, Expressions.BinaryExpression temperatureCondition, Expressions.BinaryExpression appliedStressCondition, Expressions.BinaryExpression deflectionCondition)
        {
            // Complete the BuildLambdaExpressionBody method.
            // Combine the expressions together into the body of the lambda expression.
            // Start with the dateCondition expression.
            Expressions.Expression body = null;
            if (dateCondition != null)
                body = dateCondition;

            // Add the temperatureCondition expression
            if (temperatureCondition != null)
            {
                if (body == null)
                    body = temperatureCondition;
                else
                    body = Expressions.Expression.AndAlso(body, temperatureCondition);
            }

            // Repeat the same logic for the remaining condition expressions
            if (appliedStressCondition != null)
            {
                if (body == null)
                    body = appliedStressCondition;
                else
                    body = Expressions.Expression.AndAlso(body, appliedStressCondition);
            }

            if (deflectionCondition != null)
            {
                if (body == null)
                    body = deflectionCondition;
                else
                    body = Expressions.Expression.AndAlso(body, deflectionCondition);
            }

            // If the user specified no conditions, set the body to the constant expression True.
            // NOTE: This case should not happen if this method is called correctly.
            //       It has been added to provide additional safety.
            if (body == null)
            {
                body = Expressions.Expression.Constant(true);
            }
            return body;
        }
        #endregion

        /// <summary>
        /// Method that generates a lambda expression that defines the order for presenting data.
        /// </summary>
        /// <param name="orderByKey">
        /// A member of the OrderByKey enumeration. 
        /// It specifies the key to order the data, or None if the user wants to use the default order
        /// of the TestResult type
        /// </param>
        /// <returns>
        /// An Expression that defines a lambda expression that orders data by the column specified by the user,
        /// or null if the user has not specified a field to order by.
        /// </returns>
        private Expressions.Expression<Func<TestResult, ValueType>> BuildLambdaExpressionForOrderBy(OrderByKey orderByKey)
        {
            // Define an Expression object to populate
            Expressions.Expression<Func<TestResult, ValueType>> lambda = null;

            // Verify that the user has actually specified a sort key
            if (orderByKey != OrderByKey.None)
            {
                // Create the expression that defines the parameter for the
                // lambda expression.
                // The type is TestResult, and the lambda expression refers to 
                // it with the name "item".
                // Create the type reference and ParameterExpression in the BuildLambdaExpressionForOrderBy method 
                Type testResultType = typeof(TestResult);
                Expressions.ParameterExpression itemBeingQueried = Expressions.Expression.Parameter(testResultType, "item");

                // Create the expression that will define the sort key that
                // the lambda expression returns.
                // This expression will reference one of the properties in the
                // TestResult structure depending on the key that the user
                // specifies.
                // Create a MemberExpression and MemberInfo object
                Expressions.MemberExpression sortKey = null;
                MemberInfo property = null;
                // Evaluate the orderByKey parameter to determine the property to sort by
                switch (orderByKey)
                {
                    case OrderByKey.ByDate:
                        // If the user selected the date column, set the property
                        // object to TestDate.
                        property = testResultType.GetProperty("TestDate");
                        break;
                    case OrderByKey.ByTemperature:
                        // If the user selected the temperature column,set the 
                        // property object to Temperature.
                        property = testResultType.GetProperty("Temperature");
                        break;
                    case OrderByKey.ByAppliedStress:
                        // If the user selected the applied stress column,set the 
                        // property object to AppliedStress.
                        property = testResultType.GetProperty("AppliedStress");
                        break;
                    case OrderByKey.ByDeflection:
                        // If the user selected the deflection column,set the 
                        // property object to Deflection.
                        property = testResultType.GetProperty("Deflection");
                        break;
                }

                // Construct an Expression that specifies the value in the field 
                // that the property object references in the TestResult object.
                // Construct the expression that specifies the OrderBy field
                sortKey = Expressions.Expression.MakeMemberAccess(itemBeingQueried, property);

                // Cast the sortKey object to a ValueType object (ValueType is the 
                // ancestor of all value types, including DateTime and short)
                // Create a UnaryExpression object to convert the sortKey object to a ValueType
                Expressions.UnaryExpression convert = Expressions.Expression.Convert(sortKey, typeof(ValueType));

                // Build the lambda expression by using the parameter and the 
                // expression that contains the sort key
                // Create the OrderBy lambda expression 
                lambda = Expressions.Expression.Lambda<Func<TestResult, ValueType>>(convert, itemBeingQueried);
            }

            // Return the lambda expression
            return lambda;
        }
    }
}
