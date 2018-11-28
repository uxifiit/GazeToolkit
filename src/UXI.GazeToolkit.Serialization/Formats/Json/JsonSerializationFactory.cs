using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UXI.GazeToolkit.Serialization.Converters;
using UXI.GazeToolkit.Serialization.Json.Converters;

namespace UXI.GazeToolkit.Serialization.Json
{
    public class JsonSerializationFactory : IDataSerializationFactory
    {
        private readonly bool _ignoreDefaultConverters;
        private readonly Action<JsonSerializer, DataAccess> _configureSerializerCallback;

        public JsonSerializationFactory() { }

        public JsonSerializationFactory(params JsonConverter[] converters)
           : this(converters.AsEnumerable())
        { }

        public JsonSerializationFactory(IEnumerable<JsonConverter> converters)
            : this(null, false, converters)
        { }

        public JsonSerializationFactory(Action<JsonSerializer, DataAccess> serializerConfig, params JsonConverter[] converters)
            : this(serializerConfig, converters.AsEnumerable())
        { }

        public JsonSerializationFactory(Action<JsonSerializer, DataAccess> serializerConfig, IEnumerable<JsonConverter> converters)
            : this(serializerConfig, false, converters)
        { }

        public JsonSerializationFactory(bool ignoreDefaultConverters, params JsonConverter[] converters)
            : this(ignoreDefaultConverters, converters.AsEnumerable())
        { }

        public JsonSerializationFactory(bool ignoreDefaultConverters, IEnumerable<JsonConverter> converters)
            : this(null, ignoreDefaultConverters, converters)
        { }

        public JsonSerializationFactory(Action<JsonSerializer, DataAccess> serializerConfig, bool ignoreDefaultConverters, params JsonConverter[] converters)
            : this(serializerConfig, ignoreDefaultConverters, converters.AsEnumerable())
        { }

        public JsonSerializationFactory(Action<JsonSerializer, DataAccess> serializerConfig, bool ignoreDefaultConverters, IEnumerable<JsonConverter> converters)
        {
            _configureSerializerCallback = serializerConfig;
            _ignoreDefaultConverters = ignoreDefaultConverters;

            if (converters != null)
            {
                Converters.AddRange(converters);
            }
        }


        public FileFormat Format => FileFormat.JSON;


        public List<JsonConverter> Converters { get; } = new List<JsonConverter>();


        public ReadOnlyCollection<JsonConverter> DefaultConverters { get; } = new List<JsonConverter>()
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
        }.AsReadOnly();


        public IDataReader CreateReaderForType(TextReader reader, Type dataType, SerializationConfiguration configuration)
        {
            var serializer = CreateSerializer(configuration);

            _configureSerializerCallback?.Invoke(serializer, DataAccess.Read);

            return new JsonDataReader(reader, dataType, serializer);
        }


        public IDataWriter CreateWriterForType(TextWriter writer, Type dataType, SerializationConfiguration configuration)
        {
            var serializer = CreateSerializer(configuration);

            _configureSerializerCallback?.Invoke(serializer, DataAccess.Write);

            return new JsonDataWriter(writer, serializer);
        }


        private JsonSerializer CreateSerializer(SerializationConfiguration configuration)
        {
            var serializer = CreateJsonSerializer();

            AddConverters(serializer, Converters);

            if (_ignoreDefaultConverters == false)
            {
                SetupDateTimeOffsetSerialization(serializer, configuration.TimestampConverter);
                SetupTimestampedDataSerialization(serializer, configuration.TimestampFieldName);

                AddConverters(serializer, DefaultConverters);
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


        private static void AddConverters(JsonSerializer serializer, IEnumerable<JsonConverter> converters)
        {
            foreach (var converter in converters)
            {
                serializer.Converters.Add(converter);
            }
        }


        public static JsonSerializer CreateJsonSerializer()
        {
            return new JsonSerializer()
            {
                Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US")
            };
        }
    }
}
