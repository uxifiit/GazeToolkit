using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit
{
    public class TimestampedData : ITimestampedData
    {
        public TimestampedData(DateTimeOffset timestamp)
        {
            Timestamp = timestamp;
        }


        public DateTimeOffset Timestamp { get; }
    }
}
