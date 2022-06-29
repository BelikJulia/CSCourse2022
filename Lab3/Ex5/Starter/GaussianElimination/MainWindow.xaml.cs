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

namespace GaussianElimination
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
        /// Solve the simulatneous equations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdSolve_Click(object sender, RoutedEventArgs e)
        {
            // Array to hold the coefficients of the equations
            double[,] coefficients = new double[Gauss.numberOfEquations, Gauss.numberOfEquations];

            // Array to hold the constants (Right Hand Side)
            double[] rhs = new double[Gauss.numberOfEquations];

            // Array to hold the solution values
            double[] answers = new double[Gauss.numberOfEquations];

            // Get the user entered data
            CollectAndValidateInput(coefficients, rhs);

            answers = Gauss.SolveGaussian(coefficients, rhs);

            // Display the results
            DisplayResults(answers);
        }

        /// <summary>
        /// Read the user entered data
        /// </summary>
        /// <param name="coefficients">Coefficients for all equations</param>
        /// <param name="rhs">Constants for all equations</param>
        private void CollectAndValidateInput(double[,] coefficients, double[] rhs)
        {
            if (!GetDoubleFromTextBox(w1, out coefficients[0, 0])) return;
            if (!GetDoubleFromTextBox(x1, out coefficients[0, 1])) return;
            if (!GetDoubleFromTextBox(y1, out coefficients[0, 2])) return;
            if (!GetDoubleFromTextBox(z1, out coefficients[0, 3])) return;
            if (!GetDoubleFromTextBox(w2, out coefficients[1, 0])) return;
            if (!GetDoubleFromTextBox(x2, out coefficients[1, 1])) return;
            if (!GetDoubleFromTextBox(y2, out coefficients[1, 2])) return;
            if (!GetDoubleFromTextBox(z2, out coefficients[1, 3])) return;
            if (!GetDoubleFromTextBox(w3, out coefficients[2, 0])) return;
            if (!GetDoubleFromTextBox(x3, out coefficients[2, 1])) return;
            if (!GetDoubleFromTextBox(y3, out coefficients[2, 2])) return;
            if (!GetDoubleFromTextBox(z3, out coefficients[2, 3])) return;
            if (!GetDoubleFromTextBox(w4, out coefficients[3, 0])) return;
            if (!GetDoubleFromTextBox(x4, out coefficients[3, 1])) return;
            if (!GetDoubleFromTextBox(y4, out coefficients[3, 2])) return;
            if (!GetDoubleFromTextBox(z4, out coefficients[3, 3])) return;

            if (!GetDoubleFromTextBox(r1, out rhs[0])) return;
            if (!GetDoubleFromTextBox(r2, out rhs[1])) return;
            if (!GetDoubleFromTextBox(r3, out rhs[2])) return;
            if (!GetDoubleFromTextBox(r4, out rhs[3])) return;

            // Display formatted versions of the equations
            this.equation1.Content = string.Format("{0}w + {1}x + {2}y + {3}z = {4}", coefficients[0, 0], coefficients[0, 1], coefficients[0, 2], coefficients[0, 3], rhs[0]);
            this.equation2.Content = string.Format("{0}w + {1}x + {2}y + {3}z = {4}", coefficients[1, 0], coefficients[1, 1], coefficients[1, 2], coefficients[1, 3], rhs[1]);
            this.equation3.Content = string.Format("{0}w + {1}x + {2}y + {3}z = {4}", coefficients[2, 0], coefficients[2, 1], coefficients[2, 2], coefficients[2, 3], rhs[2]);
            this.equation4.Content = string.Format("{0}w + {1}x + {2}y + {3}z = {4}", coefficients[3, 0], coefficients[3, 1], coefficients[3, 2], coefficients[3, 3], rhs[3]);
        }

        /// <summary>
        /// Try to parse the contents of a TextBox as a double.
        /// Displays a message box with the data if the text can't be parsed.
        /// </summary>
        /// <param name="textbox">TextBox to parse</param>
        /// <param name="d">Double value (as output parameter)</param>
        /// <returns>True if it succeeds, false otherwise</returns>
        private bool GetDoubleFromTextBox(TextBox textbox, out double d)
        {
            if(!double.TryParse(textbox.Text, out d)) 
            {
                MessageBox.Show("Data couldn't be parsed as a double: " + textbox.Text);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Display formatted results
        /// </summary>
        /// <param name="answers">Array containing solution</param>
        private void DisplayResults(double[] answers)
        {
            this.results.Content = string.Format("w = {0}, x = {1}, y = {2}, z = {3}", answers[0], answers[1], answers[2], answers[3]);
        }
    }
}
