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

namespace WpfApplication
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

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            string line = testInput.Text;
            line = line.Replace(",", " y:");
            line = "x:" + line;
            formattedText.Text += line + "\n";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            formattedText.Text = "";
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            string line = "";
            formattedText.Text = line;
            while ((line = Console.ReadLine()) != null)
            {
                line = line.Replace(",", " y:");
                line = "x:" + line;
                formattedText.Text += line + "\n";
            }
        }

    }
}
