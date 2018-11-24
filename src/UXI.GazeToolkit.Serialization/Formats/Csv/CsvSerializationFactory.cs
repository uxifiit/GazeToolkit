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
        public CsvSerializationFactory() { }

        public CsvSerializationFactory(IDictionary<Type, ITypeConverter> typeConverters, bool replaceDefaultConverters, params IDataConverter[] converters)
            : this(typeConverters, replaceDefaultConverters, converters.AsEnumerable())
        { }

        public CsvSerializationFactory(params IDataConverter[] converters)
            : this(null, false, converters.AsEnumerable())
        { }

        public CsvSerializationFactory(bool replaceDefaultConverters, params IDataConverter[] converters)
            : this(null, replaceDefaultConverters, converters.AsEnumerable())
        { }

        public CsvSerializationFactory(IDictionary<Type, ITypeConverter> typeConverters, bool replaceDefaultConverters, IEnumerable<IDataConverter> converters)
        {
            if (typeConverters != null)
            {
                foreach (var typeConverterPair in typeConverters)
                {
                    if (DefaultTypeConverters.ContainsKey(typeConverterPair.Key))
                    {
                        DefaultTypeConverters[typeConverterPair.Key] = typeConverterPair.Value;
                    }
                    else
                    {
                        DefaultTypeConverters.Add(typeConverterPair.Key, typeConverterPair.Value);
                    }
                }
            }

            if (replaceDefaultConverters)
            {
                DefaultConverters.Clear();
            }

            if (converters != null)
            {
                DefaultConverters.AddRange(converters);
            }
        }


        public FileFormat Format => FileFormat.CSV;


        public Dictionary<Type, ITypeConverter> DefaultTypeConverters { get; } = new Dictionary<Type, ITypeConverter>();


        public List<IDataConverter> DefaultConverters { get; } = new List<IDataConverter>()
        {
            new EyeMovementDataConverter()
            //new DefaultDataConverter()
        };


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

            SetupDateTimeOffsetSerialization(serializer, configuration.TimestampConverter);
            SetupTimestampedDataSerialization(serializer, configuration.TimestampFieldName);

            SetupDefaultConverters(serializer);

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


        private void SetupDefaultConverters(CsvSerializerContext serializer)
        {
            foreach (var typeConverter in DefaultTypeConverters)
            {
                serializer.Configuration.TypeConverterCache.AddConverter(typeConverter.Key, typeConverter.Value);
            }

            foreach (var converter in DefaultConverters)
            {
                serializer.DataConverters.Add(converter);
            }
        }
    }
}