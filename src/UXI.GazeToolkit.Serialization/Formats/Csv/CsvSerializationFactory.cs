using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Serialization.Converters;
using UXI.GazeToolkit.Serialization.Csv.DataConverters;
using UXI.GazeToolkit.Serialization.Csv.TypeConverters;

namespace UXI.GazeToolkit.Serialization.Csv
{
    public class CsvSerializationFactory : IDataSerializationFactory
    {
        private readonly bool _ignoreDefaultConverters;
        private readonly Action<Configuration, DataAccess> _configureSerializerCallback;

        public CsvSerializationFactory() { }

        public CsvSerializationFactory(params IDataConverter[] converters)
            : this(converters.AsEnumerable())
        { }

        public CsvSerializationFactory(IEnumerable<IDataConverter> converters)
            : this(null, false, converters)
        { }

        public CsvSerializationFactory(Action<Configuration, DataAccess> configureSerializer, params IDataConverter[] converters)
            : this(configureSerializer, converters.AsEnumerable())
        { }

        public CsvSerializationFactory(Action<Configuration, DataAccess> configureSerializer, IEnumerable<IDataConverter> converters)
            : this(configureSerializer, false, converters)
        { }

        public CsvSerializationFactory(bool ignoreDefaultConverters, params IDataConverter[] converters)
            : this(ignoreDefaultConverters, converters.AsEnumerable())
        { }

        public CsvSerializationFactory(bool ignoreDefaultConverters, IEnumerable<IDataConverter> converters)
            : this(null, ignoreDefaultConverters, converters)
        { }

        public CsvSerializationFactory(Action<Configuration, DataAccess> configureSerializer, bool replaceDefaultConverters, params IDataConverter[] converters)
           : this(configureSerializer, replaceDefaultConverters, converters.AsEnumerable())
        { }

        public CsvSerializationFactory(Action<Configuration, DataAccess> configureSerializer, bool ignoreDefaultConverters, IEnumerable<IDataConverter> converters)
        {
            _configureSerializerCallback = configureSerializer;
            _ignoreDefaultConverters = ignoreDefaultConverters;

            if (converters != null)
            {
                Converters.AddRange(converters);
            }
        }


        public FileFormat Format => FileFormat.CSV;


        public List<IDataConverter> Converters { get; } = new List<IDataConverter>();


        public ReadOnlyCollection<IDataConverter> DefaultConverters { get; } = new List<IDataConverter>()
        {
            new EyeMovementDataConverter(),
            new DefaultDataConverter()
        }.AsReadOnly();


        public IDataReader CreateReaderForType(TextReader reader, Type dataType, SerializationConfiguration configuration)
        {
            var serializer = CreateSerializer(configuration);

            _configureSerializerCallback?.Invoke(serializer.Configuration, DataAccess.Read);

            return new CsvDataReader(reader, dataType, serializer);
        }


        public IDataWriter CreateWriterForType(TextWriter writer, Type dataType, SerializationConfiguration configuration)
        {
            var serializer = CreateSerializer(configuration);

            _configureSerializerCallback?.Invoke(serializer.Configuration, DataAccess.Write);

            return new CsvDataWriter(writer, dataType, serializer);
        }


        private CsvSerializerContext CreateSerializer(SerializationConfiguration configuration)
        {
            var serializer = CreateCsvSerializerContext();

            AddConverters(serializer, Converters);

            if (_ignoreDefaultConverters == false)
            {
                SetupDateTimeOffsetSerialization(serializer, configuration.TimestampConverter);
                SetupTimestampedDataSerialization(serializer, configuration.TimestampFieldName);

                AddConverters(serializer, DefaultConverters);
            }

            return serializer;
        }


        private void SetupDateTimeOffsetSerialization(CsvSerializerContext serializer, ITimestampStringConverter timestampConverter)
        {
            serializer.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetTypeConverter(timestampConverter));
        }


        private void SetupTimestampedDataSerialization(CsvSerializerContext serializer, string timestampFieldName)
        {
            serializer.TimestampFieldName = timestampFieldName;
        }


        private static void AddConverters(CsvSerializerContext serializer, IEnumerable<IDataConverter> converters)
        {
            foreach (var converter in converters)
            {
                serializer.DataConverters.Add(converter);
            }
        }


        public static CsvSerializerContext CreateCsvSerializerContext()
        {
            var serializer = new CsvSerializerContext();

            serializer.Configuration.PrepareHeaderForMatch = header => header.ToLower();
            serializer.Configuration.CultureInfo = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            return serializer;
        }
    }
}