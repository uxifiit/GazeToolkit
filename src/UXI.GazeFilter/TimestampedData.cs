using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeFilter
{
    public class TimestampedData : ITimestampedData
    {
        public TimestampedData(long trackerTicks)
        {
            TrackerTicks = trackerTicks;
        }


        public long TrackerTicks { get; }
    }
}
