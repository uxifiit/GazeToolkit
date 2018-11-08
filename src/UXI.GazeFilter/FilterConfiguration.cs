using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeFilter.Serialization.Converters;

namespace UXI.GazeFilter
{
    public class FilterConfiguration
    {
        public ITimestampStringConverter TimestampConverter { get; set; }
        public string TimestampFieldName { get; set; }
    }
}
