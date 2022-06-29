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
using StressTest;

namespace Structures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TestCaseResult[] results;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create some sample Test Case Results and display
        /// in a ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void RunTests_Click(object sender, RoutedEventArgs e)
        {
            reasonsList.Items.Clear();

            // Fill the array with 10 TestCaseResult objects.
            results = new TestCaseResult[10];
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = TestManager.GenerateResult();
            }

            int passCount = 0;
            int failCount = 0;

            for (int i = 0; i < results.Length; i++)
            {
                switch (results[i].Result)
                {
                    case TestResult.Pass:
                        passCount++;
                        break;
                    case TestResult.Fail:
                        failCount++;
                        reasonsList.Items.Add(results[i].ReasonForFailure);
                        break;
                }
            }

            successLabel.Content = "Successes: " + passCount;
            failureLabel.Content = "Failures: " + failCount;
        }
    }
}
