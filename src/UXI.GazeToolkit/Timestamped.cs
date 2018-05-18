using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public class Timestamped : ITimestamped
    {
        public Timestamped(long timestamp)
        {
            Timestamp = timestamp;
        }

        public long Timestamp { get; }
    }
}
