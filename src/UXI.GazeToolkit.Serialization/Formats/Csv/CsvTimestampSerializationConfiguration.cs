using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Serialization.Converters;
using UXI.GazeToolkit.Serialization.Csv.Converters;
using UXI.GazeToolkit.Serialization.Csv.TypeConverters;
using UXI.Serialization;
using UXI.Serialization.Csv;
using UXI.Serialization.Csv.Converters;

namespace UXI.GazeToolkit.Serialization.Csv
{
    public class CsvTimestampSerializationConfiguration : SerializationConfiguration<CsvSerializerContext, SerializationSettings>
    {
        protected override CsvSerializerContext Configure(CsvSerializerContext serializer, DataAccess access, SerializationSettings settings)
        {
            if (settings != null)
            {
                SetupDateTimeOffsetSerialization(serializer, settings.TimestampConverter);
                SetupTimestampedDataSerialization(serializer, settings.TimestampFieldName);
            }

            return serializer;
        }

        private void SetupDateTimeOffsetSerialization(CsvSerializerContext serializer, ITimestampStringConverter timestampConverter)
        {
            if (timestampConverter != null)
            {
                serializer.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetTypeConverter(timestampConverter));
            }
        }


        private void SetupTimestampedDataSerialization(CsvSerializerContext serializer, string timestampFieldName)
        {
            CsvConverter converter = String.IsNullOrWhiteSpace(timestampFieldName)
                                    ? new TimestampedDataCsvConverter()
                                    : new TimestampedDataCsvConverter(timestampFieldName);

            serializer.Converters.Add(converter);
        }
    }
}
