﻿using System;
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
using MeasuringDevice;

namespace Monitor
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

        MeasureMassDevice device;

        // Declare a delegate to reference NewMeasurementEvent handler.
        EventHandler newMeasurementTaken;
        private void startCollecting_Click(object sender, RoutedEventArgs e)
        {
            if (device == null)
                device = new MeasureMassDevice(Units.Metric, "LogFile.txt");

            // Use a delegate to refer to the event handler.
            // Hook up the delegate to an event handler method.
            newMeasurementTaken = new EventHandler(device_NewMeasurementTaken);
            // Hook up the event handler to the event.
            device.NewMeasurementTaken += newMeasurementTaken;
            loggingFileNameBox.Text = device.GetLoggingFile();
            unitsBox.Text = device.UnitsToUse.ToString();

            device.StartCollecting();
         }

        // Add the device_NewMeasurementTaken event handler method to update the UI with the new measurement.
        private void device_NewMeasurementTaken(object sender, EventArgs e)
        {
            if (device != null)
            {
                mostRecentMeasureBox.Text = device.MostRecentMeasure.ToString();
                metricValueBox.Text = device.MetricValue().ToString();
                imperialValueBox.Text = device.ImperialValue().ToString();
                rawDataValues.ItemsSource = null;
                rawDataValues.ItemsSource = device.GetRawData();
            }
        }
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (device != null)
            {
                device.LoggingFileName = loggingFileNameBox.Text;
            }
            else
            {
                MessageBox.Show("You must create an instance of the MeasureMassDevice class first.");
            }
        }

        private void dispose_Click(object sender, RoutedEventArgs e)
        {
            if (device != null)
            {
                device.Dispose();
                device = null;
            }
        }

        private void stopCollecting_Click(object sender, RoutedEventArgs e)
        {
            if (device != null)
            {
                device.StopCollecting();

                // Disconnect the event handler.
                device.NewMeasurementTaken -= newMeasurementTaken;
            }
        }
    }
}
