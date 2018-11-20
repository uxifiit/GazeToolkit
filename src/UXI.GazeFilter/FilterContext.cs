using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Serialization;
using UXI.GazeToolkit.Serialization.Json;

namespace UXI.GazeFilter
{
    public class FilterContext
    {
        public Collection<IDataSerializationFactory> Formats { get; set; } = new Collection<IDataSerializationFactory>()
        {
            new JsonSerializationFactory()
        };


        public SerializationConfiguration Serialization { get; set; } = new SerializationConfiguration();


        public DataIO IO { get; set; } = null;
    }
}
