using System;
using Newtonsoft.Json.Linq;
using UXI.Serialization.Json.Extensions;
using UXI.Serialization.Json.Converters;
using Newtonsoft.Json;

namespace UXI.GazeToolkit.Serialization.Json.Converters
{
    public class TimestampedDataJsonConverter : GenericJsonConverter<ITimestampedData>
    {
        private readonly string _timestampFieldName;


        public TimestampedDataJsonConverter()
        {
            _timestampFieldName = nameof(ITimestampedData.Timestamp);
        }


        public TimestampedDataJsonConverter(string timestampFieldName)
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


        protected override ITimestampedData Convert(JToken token, JsonSerializer serializer)
        {
            JObject obj = (JObject)token;

            var timestamp = obj.GetValue<DateTimeOffset>(_timestampFieldName, serializer);

            return new TimestampedData(timestamp);
        }
    }
}
