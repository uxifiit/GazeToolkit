using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeFilter.Serialization;
using UXI.GazeFilter.Serialization.Converters;
using UXI.GazeFilter.Serialization.Json;

namespace UXI.GazeFilter
{
    public class FilterConfiguration
    {
        public Collection<IDataSerializationFactory> Formats { get; set; } = new Collection<IDataSerializationFactory>()
        {
            new JsonSerializationFactory()
        };

        public ITimestampStringConverter TimestampConverter { get; set; }

        public string TimestampFieldName { get; set; }
    }
}
