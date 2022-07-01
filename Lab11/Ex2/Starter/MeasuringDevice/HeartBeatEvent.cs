using System;

namespace MeasuringDevice
{
    public class HeartBeatEventArgs : EventArgs
    {
        public DateTime TimeStamp { get; private set; }
        public HeartBeatEventArgs() : base()
        {
            this.TimeStamp = DateTime.Now;
        }
    }
    public delegate void HeartBeatEventHandler(object sender, HeartBeatEventArgs args);
}