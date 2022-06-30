using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeviceControl;


namespace MeasuringDevice
{
    public abstract class MeasureDataDevice : IMeasuringDevice
    {
        protected Units unitsToUse;
        protected int[] dataCaptured;
        protected int mostRecentMeasure;
        protected DeviceController controller;
        protected DeviceType measurementType;

        public abstract decimal MetricValue();
        public abstract decimal ImperialValue();

        /// <summary>
        /// Starts the measuring device.
        /// </summary>
        public void StartCollecting()
        {
            controller = DeviceController.StartDevice(measurementType);
            GetMeasurements();
        }
        /// <summary>
        /// Stops the measuring device.
        /// </summary>
        public void StopCollecting()
        {
            if (controller != null)
            {
                controller.StopDevice();
                controller = null;
            }
        }
        /// <summary>
        /// Enables access to the raw data from the device in whatever units are native to the device.
        /// </summary>
        /// <returns>The raw data from the device in native format.</returns>
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
                    System.Threading.Thread.Sleep(
                    timer.Next(1000, 5000));
                    dataCaptured[x] = controller != null ?
                    controller.TakeMeasurement()
                    : dataCaptured[x];
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
