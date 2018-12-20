using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Serialization.Converters;
using UXI.GazeToolkit.Serialization.Json.Converters;
using UXI.Serialization;

namespace UXI.GazeToolkit.Serialization.Json
{
    public class JsonTimestampSerializationConfiguration : SerializationConfiguration<JsonSerializer, SerializationSettings>
    {
        protected override JsonSerializer Configure(JsonSerializer serializer, DataAccess access, SerializationSettings settings)
        {
            if (settings != null)
            {
                SetupDateTimeOffsetSerialization(serializer, settings.TimestampConverter);
                SetupTimestampedDataSerialization(serializer, settings.TimestampFieldName);
            }

            return serializer;
        }

        private void SetupDateTimeOffsetSerialization(JsonSerializer serializer, ITimestampStringConverter timestampConverter)
        {
            serializer.DateTimeZoneHandling = DateTimeZoneHandling.Local;

            if (timestampConverter == null)
            {
                serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                serializer.DateParseHandling = DateParseHandling.DateTimeOffset;
            }
            else
            {
                serializer.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                serializer.DateParseHandling = DateParseHandling.None;

                serializer.Converters.Add(new DateTimeOffsetJsonConverter(timestampConverter));
            }
        }


        private void SetupTimestampedDataSerialization(JsonSerializer serializer, string timestampFieldName)
        {
            JsonConverter converter = String.IsNullOrWhiteSpace(timestampFieldName)
                                    ? new TimestampedDataJsonConverter()
                                    : new TimestampedDataJsonConverter(timestampFieldName);

            serializer.Converters.Add(converter);
        }

    }
}
