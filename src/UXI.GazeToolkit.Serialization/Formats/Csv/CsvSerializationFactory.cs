using CsvHelper;
using CsvHelper.TypeConversion;
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

        public CsvSerializationFactory() { }

        public CsvSerializationFactory(IDictionary<Type, ITypeConverter> typeConverters, bool replaceDefaultConverters, params IDataConverter[] converters)
            : this(typeConverters, replaceDefaultConverters, converters.AsEnumerable())
        { }

        public CsvSerializationFactory(params IDataConverter[] converters)
            : this(null, false, converters.AsEnumerable())
        { }

        public CsvSerializationFactory(bool ignoreDefaultConverters, params IDataConverter[] converters)
            : this(null, ignoreDefaultConverters, converters.AsEnumerable())
        { }

        public CsvSerializationFactory(IDictionary<Type, ITypeConverter> typeConverters, bool ignoreDefaultConverters, IEnumerable<IDataConverter> converters)
        {
            if (typeConverters != null)
            {
                foreach (var typeConverterPair in typeConverters)
                {
                    if (TypeConverters.ContainsKey(typeConverterPair.Key))
                    {
                        TypeConverters[typeConverterPair.Key] = typeConverterPair.Value;
                    }
                    else
                    {
                        TypeConverters.Add(typeConverterPair.Key, typeConverterPair.Value);
                    }
                }
            }

            _ignoreDefaultConverters = ignoreDefaultConverters;

            if (converters != null)
            {
                Converters.AddRange(converters);
            }
        }


        public FileFormat Format => FileFormat.CSV;


        public Dictionary<Type, ITypeConverter> TypeConverters { get; } = new Dictionary<Type, ITypeConverter>();


        public List<IDataConverter> Converters { get; } = new List<IDataConverter>();


        public ReadOnlyCollection<IDataConverter> DefaultConverters { get; } = new List<IDataConverter>()
        {
            new EyeMovementDataConverter(),
            new DefaultDataConverter()
        }.AsReadOnly();


        public IDataReader CreateReaderForType(TextReader reader, Type dataType, SerializationConfiguration configuration)
        {
            var serializer = CreateSerializer(configuration);

            return new CsvDataReader(reader, dataType, serializer);
        }


        public IDataWriter CreateWriterForType(TextWriter writer, Type dataType, SerializationConfiguration configuration)
        {
            var serializer = CreateSerializer(configuration);

            return new CsvDataWriter(writer, dataType, serializer);
        }


        private CsvSerializerContext CreateSerializer(SerializationConfiguration configuration)
        {
            var serializer = new CsvSerializerContext();

            serializer.Configuration.PrepareHeaderForMatch = header => header.ToLower();

            AddConverters(serializer, Converters);
            AddTypeConverters(serializer);

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


        private void AddTypeConverters(CsvSerializerContext serializer)
        {
            foreach (var typeConverter in TypeConverters)
            {
                serializer.Configuration.TypeConverterCache.AddConverter(typeConverter.Key, typeConverter.Value);
            }
        }

        private static void AddConverters(CsvSerializerContext serializer, IEnumerable<IDataConverter> converters)
        {
            foreach (var converter in converters)
            {
                serializer.DataConverters.Add(converter);
            }
        }
    }
}