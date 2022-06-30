using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeviceControl;


namespace MeasuringDevice
{
    public class MeasureMassDevice : MeasureDataDevice, IMeasuringDevice
    {
        public MeasureMassDevice(Units deviceUnits)
        {
            unitsToUse = deviceUnits;
            measurementType = DeviceType.MASS;
        }
        public override decimal MetricValue()
        {
            return (unitsToUse == Units.Metric) ? (decimal)mostRecentMeasure : (decimal)mostRecentMeasure * 0.4536M;
        }

        public override decimal ImperialValue()
        {
            return (unitsToUse == Units.Metric) ? (decimal)mostRecentMeasure : (decimal)mostRecentMeasure * 2.2046M;
        }
    }
}
