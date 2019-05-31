using System;
using UXI.GazeToolkit.Serialization.Csv.Converters;
using UXI.Serialization;
using UXI.Serialization.Configurations;
using UXI.Serialization.Formats.Csv;
using UXI.Serialization.Formats.Csv.Converters;

namespace UXI.GazeToolkit.Serialization.Csv
{
    public class CsvTimestampedDataSerializationConfiguration : SerializationConfiguration<CsvSerializerContext>
    {
        public CsvTimestampedDataSerializationConfiguration(string timestampFieldName)
        {
            TimestampFieldName = timestampFieldName;
        }


        public string TimestampFieldName { get; set; }


        protected override CsvSerializerContext Configure(CsvSerializerContext serializer, DataAccess acess, object settings)
        {
            SetupTimestampedDataSerialization(serializer, TimestampFieldName);

            return serializer;
        }

        // TODO Change to AddOrUpdate, not just Add
        private void SetupTimestampedDataSerialization(CsvSerializerContext serializer, string timestampFieldName)
        {
            CsvConverter converter = String.IsNullOrWhiteSpace(timestampFieldName)
                                    ? new TimestampedDataCsvConverter()
                                    : new TimestampedDataCsvConverter(timestampFieldName);

            serializer.Converters.Add(converter);
        }
    }
}
