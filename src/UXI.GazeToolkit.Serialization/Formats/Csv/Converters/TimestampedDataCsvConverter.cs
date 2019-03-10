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


        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref ITimestampedData result)
        {
            DateTimeOffset timestamp; 

            if (reader.TryGetField<DateTimeOffset>(naming.Get(_timestampFieldName), out timestamp))
            {
                result = new TimestampedData(timestamp);
                return true;
            }

            return false;
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(_timestampFieldName));
        }
       

        protected override void Write(ITimestampedData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Timestamp);
        }
    }
}
