using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UXI.GazeFilter.Serialization.Converters;
using UXI.GazeFilter.Serialization.Json.Converters;
using Newtonsoft.Json.Converters;

namespace UXI.GazeFilter.Serialization.Json
{
    public class JsonSerializationFactory : IDataSerializationFactory
    {
        public JsonSerializationFactory() { }


        public JsonSerializationFactory(IEnumerable<JsonConverter> converters, bool replaceDefaultConverters = false)
        {
            if (replaceDefaultConverters)
            {
                DefaultConverters.Clear();
            }

            DefaultConverters.AddRange(converters);
        }


        public string FileExtension => "json";


        public FileFormat Format => FileFormat.JSON;


        public List<JsonConverter> DefaultConverters { get; } = new List<JsonConverter>()
        {
            new Point2Converter(),
            new Point3Converter(),
            new EyeSampleConverter(),
            new EyeDataConverter(),
            new GazeDataConverter(),
            new SingleEyeGazeDataJsonConverter(),
            new EyeVelocityJsonConverter(),
            new EyeMovementJsonConverter(),
            new StringEnumConverter(false)
        };


        public IDataReader CreateReaderForType(TextReader reader, Type dataType, FilterConfiguration configuration)
        {
            var serializer = CreateSerializer(configuration);

            return new JsonDataReader(reader, dataType, serializer);
        }


        public IDataWriter CreateWriterForType(TextWriter writer, Type dataType, FilterConfiguration configuration)
        {
            var serializer = CreateSerializer(configuration);

            return new JsonDataWriter(writer, serializer);
        }


        private JsonSerializer CreateSerializer(FilterConfiguration configuration)
        {
            var serializer = new JsonSerializer();
           
            SetupDateTimeOffsetSerialization(serializer, configuration.TimestampConverter);
            SetupTimestampedDataSerialization(serializer, configuration.TimestampFieldName);

            SetupDefaultConverters(serializer);

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


        private void SetupDefaultConverters(JsonSerializer serializer)
        {
            foreach (var converter in DefaultConverters)
            {
                serializer.Converters.Add(converter);
            }
        }
    }
}
