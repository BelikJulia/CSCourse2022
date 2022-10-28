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
using System.Diagnostics;

namespace GreatestCommonDivisor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Do the GCD calculations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindGCD_Click(object sender, RoutedEventArgs e)
        {
            int firstNumber;
            int secondNumber;
            int thirdNumber;
            int fourthNumber;
            int fifthNumber;

            if (!GetPostiveIntegerFromTextBox(integer1, out firstNumber)) return;
            if (!GetPostiveIntegerFromTextBox(integer2, out secondNumber)) return;
            if (!GetPostiveIntegerFromTextBox(integer3, out thirdNumber)) return;
            if (!GetPostiveIntegerFromTextBox(integer4, out fourthNumber)) return;
            if (!GetPostiveIntegerFromTextBox(integer5, out fifthNumber)) return;

            if (sender == findGCD) // Euclid for two integers
            {
                long timeEuclid, timeStein;
                // Do the calculations
                this.resultEuclid.Content = String.Format("Euclid: {0}, Time (ticks): {1}", GCDAlgorithms.FindGCDEuclid(firstNumber, secondNumber, out timeEuclid), timeEuclid);

                this.resultStein.Content = String.Format("Stein: {0}, Time (ticks): {1}", GCDAlgorithms.FindGCDStein(firstNumber, secondNumber, out timeStein), timeStein);
            }
            else if (sender == findGCD3) // Euclid for three integers
            {
                this.resultEuclid.Content = String.Format("Euclid: {0}", GCDAlgorithms.FindGCDEuclid(firstNumber, secondNumber, thirdNumber));
                this.resultStein.Content = "N/A";
            }
            else if (sender == findGCD4) // Euclid for four integers
            {
                this.resultEuclid.Content = String.Format("Euclid: {0}", GCDAlgorithms.FindGCDEuclid(firstNumber, secondNumber, thirdNumber, fourthNumber));
                this.resultStein.Content = "N/A";
            }
            else if (sender == findGCD5) // Euclid for five integers
            {
                this.resultEuclid.Content = String.Format("Euclid: {0}", GCDAlgorithms.FindGCDEuclid(firstNumber, secondNumber, thirdNumber, fourthNumber, fifthNumber));
                this.resultStein.Content = "N/A";
            }
        }

        /// <summary>
        /// Read a postive integer from a TextBox
        /// Displays a message box with the data if the text can't be parsed.
        /// </summary>
        /// <param name="textBox">TextBox to read</param>
        /// <param name="i">Postive integer (out parameter)</param>
        /// <returns>True if success, false otherwise</returns>
        private bool GetPostiveIntegerFromTextBox(TextBox textBox, out int i)
        {
            i = -1;
            if (int.TryParse(textBox.Text, out i))
            {
                if (i >= 0) return true;
            }
            MessageBox.Show("Not a positive integer value: " + textBox.Text);
            return false;
        }

        /// <summary>
        /// Display the results in a simple graph
        /// </summary>
        /// <param name="euclidTime">Time taken by Euclid algorithm</param>
        /// <param name="steinTime">Time taken by Stein algorithm</param>
        private void DrawGraph(long euclidTime, long steinTime)
        {
            // Clear the canvas before we start
            chartCanvas.Children.Clear();

            Orientation orientation = Orientation.Horizontal;

            double euclidProportion;
            double steinProportion;

            // Get brushes in requested colors
            BrushConverter bc = new BrushConverter();
            Brush bEuclid = Brushes.Green;
            Brush bStein = Brushes.Red;

            // Create two colored rectangles
            Rectangle rEuclid = new Rectangle();
            rEuclid.Stroke = bEuclid;
            rEuclid.Fill = bEuclid;
            rEuclid.VerticalAlignment = VerticalAlignment.Bottom;
            rEuclid.HorizontalAlignment = HorizontalAlignment.Left;

            Rectangle rStein = new Rectangle();
            rStein.Stroke = bStein;
            rStein.Fill = bStein;
            rStein.VerticalAlignment = VerticalAlignment.Bottom;
            rStein.HorizontalAlignment = HorizontalAlignment.Left;

            // Calculate relative sizes (largest = 1)
            if (euclidTime > steinTime)
            {
                euclidProportion = 1;
                steinProportion = (double)steinTime / (double)euclidTime;
            }
            else if (euclidTime < steinTime)
            {
                steinProportion = 1;
                euclidProportion = (double)euclidTime / (double)steinTime;
            }
            else
            {
                euclidProportion = steinProportion = 1;
            }

            // Calculate rectangle sizes and orientation
            chartCanvas.Orientation = orientation;
            if (orientation == Orientation.Horizontal)
            {
                rEuclid.Height = chartCanvas.ActualHeight * euclidProportion;
                rStein.Height = chartCanvas.ActualHeight * steinProportion;
                rEuclid.Width = chartCanvas.ActualWidth / 2;
                rStein.Width = chartCanvas.ActualWidth / 2;
            }
            else
            {
                rEuclid.Width = chartCanvas.ActualWidth * euclidProportion;
                rStein.Width = chartCanvas.ActualWidth * steinProportion;
                rEuclid.Height = chartCanvas.ActualHeight / 2;
                rStein.Height = chartCanvas.ActualHeight / 2;
            }

            // Add the rectangles to the chart
            chartCanvas.Children.Add(rEuclid);
            chartCanvas.Children.Add(rStein);
        }
    }
}
