using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeFilter.Statistics;
using UXI.GazeToolkit.Serialization;
using UXI.GazeToolkit.Serialization.Csv;
using UXI.GazeToolkit.Serialization.Json;

namespace UXI.GazeFilter
{
    public class FilterContext
    {
        public Collection<IDataSerializationFactory> Formats { get; set; } = new Collection<IDataSerializationFactory>()
        {
            new JsonSerializationFactory(),
            new CsvSerializationFactory()
        };


        public Collection<IFilterStatisticsFactory> Statistics { get; set; } = new Collection<IFilterStatisticsFactory>()
        {
            new SamplesCounterStatisticsFactory()
        };


        public SerializationConfiguration Serialization { get; set; } = new SerializationConfiguration();


        public DataIO IO { get; set; } = null;
    }
}
