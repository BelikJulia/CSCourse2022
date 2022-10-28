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

// Add Namespaces containing IronPython and IronRuby runtime support and interop types
using IronPython.Hosting;
using IronRuby;
using Microsoft.Scripting.Hosting;

namespace DynamicLanguageInterop
{
    /// <summary>
    /// WPF application to demonstrate C# interoperability with Python and Ruby.
    /// <para>
    /// The Python script implements the Fisher-Yates-Durstenfeld algorithm to randomly shuffle data in a collection. 
    /// It is used for generating random sequences with a specified set of values.
    /// </para>
    /// The Ruby script implements a trapezoid class. It is used for architectural modelling.
    /// </summary>
    public partial class InteropTestWindow : Window
    {
        /// <summary>
        /// pythonLibPath holds the name of the folder containing the modules provided with Python.
        /// <para>
        /// pythonCode and rubyCode hold the paths to the Python and Ruby scripts used by this application.
        /// </para>
        /// </summary>
        private const string pythonLibPath = @"C:\Program Files\IronPython 2.6 for .NET 4.0\Lib";
        private const string pythonCode = @"E:\Labfiles\Lab 15\Python\Shuffler.py";
        private const string rubyCode = @"E:\Labfiles\Lab 15\Ruby\Trapezoid.rb";

        public InteropTestWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event-handling method for the Click event of the Shuffle button on the Python Test tab.
        /// <para>
        /// This method reads the data provided by the user, divides it into a list of words or numbers, and stores this list in an array.
        /// The method then calls the ShuffleData method to randomly shuffle this data, and then displays the result.
        /// </para>
        /// </summary>
        private void shuffle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // pythonTestData is a TextBox that holds the user input.
                // This can be a series of words or numbers, seperated by spaces.
                // inputData is populated with an array of strings, where each string is the text of a word or number.
                string[] inputData = this.pythonTestData.Text.Split(' ');

                // data will hold a copy of the strings from inputData and convert them to numbers
                // depending on whether the user specified that the data was numeric or text
                object[] data = new object[inputData.Length];

                if (this.integer.IsChecked.Value)
                {
                    for (int n = 0; n < inputData.Length; n++)
                    {
                        data[n] = Int32.Parse(inputData[n]);
                    }
                }
                else
                {
                    inputData.CopyTo(data, 0);
                }

                // Shuffle the data array
                ShuffleData(data);

                // Display the shuffled results in the shuffledData TextBox
                this.shuffledData.Clear();
                StringBuilder result = new StringBuilder();
                foreach (var item in data)
                {
                    
                    result.Append(item.ToString());
                    result.Append(' ');
                }

                this.shuffledData.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// This method calls the Python script to create an instance of the Shuffler class and shuffle the data.
        /// </summary>
        /// <param name="data">
        /// The array containing the data to be shuffled.
        /// </param>
        private void ShuffleData(object[] data)
        {
            // Create an instance of the Python runtime, and add a reference to the folder holding the "random" module.
            // (The Python script references this module)
            ScriptEngine pythonEngine = Python.CreateEngine();
            ICollection<string> paths = pythonEngine.GetSearchPaths();
            paths.Add(pythonLibPath);
            pythonEngine.SetSearchPaths(paths);
            // Run the script and create an instance of the Shuffler class by using the CreateShuffler method in the script.
            dynamic pythonScript = pythonEngine.ExecuteFile(pythonCode);
            dynamic pythonShuffler = pythonScript.CreateShuffler();
            // Shuffle the data  
            pythonShuffler.Shuffle(data);
        }

        /// <summary>
        /// Event-handling method for the Visualize button on the Ruby Test tab.
        /// <para>
        /// This method retrieves the parameters that define a trapezoid from the slider controls
        /// on the form, and call the CreateTrapezoid method to create an instance of the Ruby
        /// Trapezoid class using these values.
        /// </para>
        /// <para>
        /// A graphical representation of the trapezoid is displayed on a canvas in the lower part of the tab.
        /// </para>
        /// </summary>
        private void visualize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve the values specified by the user. These values are used to create the trapezoid.
                int vertexAInDegrees = (int)vertexA.Value;
                int lengthSideAB = (int)sideAB.Value;
                int lengthSideCD = (int)sideCD.Value;
                int heightOfTrapezoid = (int)height.Value;
                // Call the CreateTrapezoid method and build a trapezoid object.
                dynamic trapezoid = CreateTrapezoid(vertexAInDegrees, lengthSideAB, lengthSideCD, heightOfTrapezoid);
                // Display the lengths of each side, the internal angles, and the area of the trapezoid.
                DisplayStatistics(trapezoid, this.trapezoidStatistics);
                // Display a graphical representation of the trapezoid. 
                RenderTrapezoid(trapezoid, this.trapezoidCanvas);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// This method displays the length of each side of the trapezoid, together with the internal angles, and the area.
        /// The results are displayed in a formatted TextBlock control.
        /// </summary>
        /// <param name="trapezoid">
        /// The trapezoid to display the details for.
        /// </param>
        /// <param name="trapezoidStatistics">
        /// The TextBlock control to display the results in.
        /// </param>
        private void DisplayStatistics(dynamic trapezoid, TextBlock trapezoidStatistics)
        {
            // Use a StringBuilder object to construct a string holding the details of the trapezoid.
            StringBuilder builder = new StringBuilder();
            // Call the to_s method of the trapezoid object to return the details of the trapezoid as a string.
            // Note: The ToString method invokes to_s.
            builder.Append(trapezoid.ToString());
            // Calculate the area of the trapezoid by using the area method of the trapezoid class
            // and append it to the string holding the details of the trapezoid
            builder.Append(String.Format("\nArea:\t\t{0}", trapezoid.area().ToString()));
            // Display the details of the trapezoid in the TextBlock control
            trapezoidStatistics.Text = builder.ToString();
        }

        /// <summary>
        /// This method displays a graphical representation of a trapezoid on a canvas.
        /// </summary>
        /// <param name="trapezoid">
        /// The trapezoid to display.
        /// </param>
        /// <param name="renderCanvas">
        /// The canvas to display the trapezoid on.
        /// </param>
        private void RenderTrapezoid(dynamic trapezoid, Canvas renderCanvas)
        {
            // Draw the trapezoid by using a filled polygon.
            Polygon renderedTrapezoid = new Polygon();
            renderedTrapezoid.Stroke = Brushes.DeepSkyBlue;
            renderedTrapezoid.StrokeThickness = 1;
            renderedTrapezoid.Fill = Brushes.DeepSkyBlue;

            // Define the first point of the polygon (Vertex A) to point 1,1 
            Point pointOfVertexA = new Point(1, 1);

            // Define the second point of the polygon (Vertex B).
            // The side AB forms the base of the polygon, so Vertex B has the same y-coordinate as
            // Vertex A, and the x-coordinate is the length of the base of the trapezoid.
            Point pointOfVertexB = new Point(trapezoid.sideAB, 1);

            // Define the third point of the polygon (Vertex C).
            // The x-coordinate depends on the angle of Vertex B and the length of side BC:
            //  
            //           D----------------C
            //          /|                |\
            //         / |                | \
            //        /  |h              h|  \
            //       /   |                |   \
            //      A----F----------------E----B
            // 
            // Using trigonomtry, length of sideEB = h / tan(Vertex B)
            // 
            // The x-coordinate of Vertex C (side AE) is length of sideAB - length of sideEB
            // The y-coordinate of Vertex C is h
            double vertexBInRadians = trapezoid.vertexB * Math.PI / 180;
            double xCoordOfPointC = trapezoid.sideAB - (trapezoid.h / Math.Tan(vertexBInRadians));
            Point pointOfVertexC = new Point(xCoordOfPointC, trapezoid.h);

            // Define the fourth point of the polygon (Vertex D).
            // The x-coordinate of Vertex D is the x-coordinate of Vertex C - the length of sideCD.
            // The y-coordinate of Vertex D is h
            double xCoordOfPointD = xCoordOfPointC - trapezoid.sideCD;
            Point pointOfVertexD = new Point(xCoordOfPointD, trapezoid.h);

            // Add the four points to the polygon
            renderedTrapezoid.Points.Add(pointOfVertexA);
            renderedTrapezoid.Points.Add(pointOfVertexB);
            renderedTrapezoid.Points.Add(pointOfVertexC);
            renderedTrapezoid.Points.Add(pointOfVertexD);

            // By default, the polygon will appear upside down and in the wrong place on the canvas,
            // so it needs to be rotated, translated, and scaled to fit
            Transform rotateTransform = new RotateTransform(180);
            Transform translateTransform = new TranslateTransform(renderCanvas.Width, renderCanvas.Height+ 40);
            Transform scaleTransform = new ScaleTransform(1.25, 1.25, renderCanvas.Width * 2, renderCanvas.Height * 2);
            TransformGroup transformations = new TransformGroup();
            transformations.Children.Add(rotateTransform);
            transformations.Children.Add(translateTransform);
            transformations.Children.Add(scaleTransform);
            renderedTrapezoid.RenderTransform = transformations;

            // Display the polygon on the canvas
            renderCanvas.Children.Clear();
            renderCanvas.Children.Add(renderedTrapezoid);

            // Label each of the vertices
            // (The positioning is only approximate in this example)
            // Start with Vertex A in the bottom left
            Label labelA = new Label();
            Canvas.SetLeft(labelA, 400);
            Canvas.SetTop(labelA, 260);
            labelA.Content = "A";
            labelA.FontFamily = new FontFamily("Book Antiqua");
            labelA.FontStyle = FontStyles.Italic;
            labelA.FontWeight = FontWeights.Bold;
            renderCanvas.Children.Add(labelA);

            // Vertex B is in the bottom right.
            // The x-coordinate (the Left property) depends on the width of the trapezoid
            Label labelB = new Label();
            Canvas.SetLeft(labelB, 250 - trapezoid.sideAB);
            Canvas.SetTop(labelB, 260);
            labelB.Content = "B";
            labelB.FontFamily = new FontFamily("Book Antiqua");
            labelB.FontStyle = FontStyles.Italic;
            labelB.FontWeight = FontWeights.Bold;
            renderCanvas.Children.Add(labelB);

            // Vertex C is in the top right.
            // The x-coordinate (Left property) is the same as that for Vertex B
            // The y-coordinate (Top property) depends on the height of the trapezoid
            Label labelC = new Label();
            Canvas.SetLeft(labelC, 250 - trapezoid.sideAB);
            Canvas.SetTop(labelC, 225 - trapezoid.h);
            labelC.Content = "C";
            labelC.FontFamily = new FontFamily("Book Antiqua");
            labelC.FontStyle = FontStyles.Italic;
            labelC.FontWeight = FontWeights.Bold;
            renderCanvas.Children.Add(labelC);

            // Vertex D is the top left.
            // The x-coordinate (Left property) as the same as that for Vertex A
            // The y-coordinate (Top property) as the same as that for Vertex C
            Label labelD = new Label();
            Canvas.SetLeft(labelD, 400);
            Canvas.SetTop(labelD, 225 - trapezoid.h);
            labelD.Content = "D";
            labelD.FontFamily = new FontFamily("Book Antiqua");
            labelD.FontStyle = FontStyles.Italic;
            labelD.FontWeight = FontWeights.Bold;
            renderCanvas.Children.Add(labelD);                    
        }

        /// <summary>
        /// This method calls the Ruby script to create a trapezoid object.
        /// </summary>
        /// <param name="vertexAInDegrees">
        /// The angle of Vertex A, in degrees.
        /// </param>
        /// <param name="lengthSideAB">
        /// The length of the base of the trapezoid.
        /// </param>
        /// <param name="lengthSideCD">
        /// The length of the top edge of the trapezoid.
        /// </param>
        /// <param name="heightOfTrapezoid">
        /// The height of the trapezoid.
        /// </param>
        /// <returns>
        /// A trapezoid object.
        /// </returns>
        private dynamic CreateTrapezoid(int vertexAInDegrees, int lengthSideAB, int lengthSideCD, int heightOfTrapezoid)
        {
            throw new NotImplementedException();
            // Create an instance of the Ruby runtime.
            ScriptRuntime rubyRuntime = Ruby.CreateRuntime();
            // Run the Ruby script that defines the Trapezoid class.
            dynamic rubyScript = rubyRuntime.UseFile(rubyCode);
            // Call the CreateTrapezoid method in the Ruby script to create a trapezoid object.
            dynamic rubyTrapezoid = rubyScript.CreateTrapezoid(vertexAInDegrees, lengthSideAB, lengthSideCD, heightOfTrapezoid);
            // Return the trapezoid object.     
            return rubyTrapezoid;
        }
    }
}
