using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public class Timestamped : ITimestamped
    {
        public Timestamped(TimeSpan timestamp)
        {
            Timestamp = timestamp;
        }

        public TimeSpan Timestamp { get; }
    }
}
