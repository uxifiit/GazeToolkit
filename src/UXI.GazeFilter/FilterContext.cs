using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeFilter.Statistics;
using UXI.GazeFilter.Statistics.Serialization;
using UXI.GazeToolkit.Serialization;
using UXI.GazeToolkit.Serialization.Csv;
using UXI.GazeToolkit.Serialization.Json;
using UXI.Serialization;
using UXI.Serialization.Csv;
using UXI.Serialization.Json;

namespace UXI.GazeFilter
{
    public class FilterContext
    {
        public Collection<ISerializationFactory> Formats { get; set; } = new Collection<ISerializationFactory>()
        {
            new JsonSerializationFactory
            (
#if DEBUG
                new RelaySerializationConfiguration<Newtonsoft.Json.JsonSerializer>((serializer, access, _) =>
                {
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

                    return serializer;
                }),
#endif
                new JsonTimestampSerializationConfiguration(), 
                new JsonDataConvertersSerializationConfiguration()
            ),
            new CsvSerializationFactory
            (
                new CsvTimestampSerializationConfiguration(),
                new CsvDataConvertersSerializationConfiguration(),
                new CsvConvertersSerializationConfiguration(new SamplesCountCsvConverter())
            )
        };


        public Collection<IFilterStatisticsFactory> Statistics { get; set; } = new Collection<IFilterStatisticsFactory>()
        {
            new SamplesCounterStatisticsFactory()
        };


        public SerializationSettings Serialization { get; set; } = new SerializationSettings();


        public DataIO IO { get; set; } = null;
    }
}
