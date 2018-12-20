using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Serialization.Converters;
using UXI.GazeToolkit.Serialization.Json;

namespace UXI.GazeToolkit.Serialization
{
    public class SerializationSettings
    {
        public ITimestampStringConverter TimestampConverter { get; set; }


        public string TimestampFieldName { get; set; }
    }
}
