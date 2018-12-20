using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using UXI.Serialization.Csv;
using UXI.Serialization.Csv.Converters;

namespace UXI.GazeToolkit.Serialization.Csv.Converters
{
    public class TimestampedDataCsvConverter : CsvConverter<ITimestampedData>
    {
        private readonly string _timestampFieldName;


        public TimestampedDataCsvConverter()
        {
            _timestampFieldName = nameof(TimestampedData.Timestamp);
        }


        public TimestampedDataCsvConverter(string timestampFieldName)
        {
            string fieldName = timestampFieldName?.Trim();
            if (String.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentException("Timestamp field name cannot be null, empty or white space only.", nameof(timestampFieldName));
            }

            _timestampFieldName = fieldName;
        }


        public override bool CanConvert(Type objectType)
        {
            return base.CanConvert(objectType) || objectType == typeof(TimestampedData);
        }


        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(_timestampFieldName));
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var timestamp = reader.GetField<DateTimeOffset>(naming.Get(_timestampFieldName));

            return new TimestampedData(timestamp);
        }

       
        protected override void WriteCsv(ITimestampedData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Timestamp);
        }
    }
}
