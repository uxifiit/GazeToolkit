using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public class TimestampedData : ITimestampedData
    {
        public TimestampedData(long trackerTicks, TimeSpan timestamp)
        {
            TrackerTicks = trackerTicks;
            Timestamp = timestamp;
        }

        public long TrackerTicks { get; }

        public TimeSpan Timestamp { get; }
    }
}
