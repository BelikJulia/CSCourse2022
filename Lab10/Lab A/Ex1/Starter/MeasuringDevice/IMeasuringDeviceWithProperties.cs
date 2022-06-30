using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeasuringDevice
{
    interface IMeasuringDeviceWithProperties : ILoggingMeasuringDevice
    {
        Units UnitsToUse { get; }
        int[] DataCaptured { get; }
        int MostRecentMeasure { get; }
        string LoggingFileName { get; set; }
    }
}
