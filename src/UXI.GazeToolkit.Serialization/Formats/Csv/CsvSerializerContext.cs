using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using UXI.GazeToolkit.Serialization.Converters;
using UXI.GazeToolkit.Serialization.Csv.DataConverters;

namespace UXI.GazeToolkit.Serialization.Csv
{
    public class CsvSerializerContext
    {
        public Configuration Configuration { get; set; } = new CsvHelper.Configuration.Configuration();

        public Collection<IDataConverter> DataConverters { get; set; } = new Collection<IDataConverter>();

        public ITimestampStringConverter TimestampConverter { get; set; }

        public string TimestampFieldName { get; set; }
    }
}
