﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeviceControl;

namespace MeasuringDevice
{
    public class MeasureLengthDevice : IMeasuringDevice
    {
        private Units unitsToUse;
        private int[] dataCaptured;
        private int mostRecentMeasure;
        private DeviceController controller;
        private const DeviceType measurementType = DeviceType.LENGTH;
        public MeasureLengthDevice(Units deviceUnits)
        {
            unitsToUse = deviceUnits;
        }
        public decimal MetricValue()
        {
            return (unitsToUse == Units.Metric) ? (decimal)mostRecentMeasure : (decimal)mostRecentMeasure * 25.4M;
        }

        public decimal ImperialValue()
        {
            return (unitsToUse == Units.Metric) ? (decimal)mostRecentMeasure : (decimal)mostRecentMeasure * 0.03937M;
        }

        public void StartCollecting()
        {
            controller = DeviceController.StartDevice(measurementType);
            GetMeasurements();
        }

        public void StopCollecting()
        {
            if (controller != null)
            {
                controller.StopDevice();
                controller = null;
            }
        }

        public int[] GetRawData()
        {
            return dataCaptured;
        }
        private void GetMeasurements()
        {
            dataCaptured = new int[10];
            System.Threading.ThreadPool.QueueUserWorkItem((dummy) =>
            {
                int x = 0;
                Random timer = new Random();
                while (controller != null)
                {
                    System.Threading.Thread.Sleep(timer.Next(1000, 5000));
                    dataCaptured[x] = controller != null ?
                    controller.TakeMeasurement() : dataCaptured[x];
                    mostRecentMeasure = dataCaptured[x];
                    x++;
                    if (x == 10)
                    {
                        x = 0;
                    }
                }
            });
        }
    }
}
