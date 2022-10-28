using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeasuringDevice
{
    interface IEventEnabledMeasuringDevice : IMeasuringDevice
    {
        event EventHandler NewMeasurementTaken;

        // Define the new event in the interface.
        // Event that fires every heartbeat.
        event HeartBeatEventHandler HeartBeat;
        // Define the HeartBeatInterval property in the interface.
        // Read only heartbeat interval - set in constructor.
        int HeartBeatInterval { get; }
    }
}
