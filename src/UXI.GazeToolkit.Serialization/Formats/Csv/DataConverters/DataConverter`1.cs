using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace UXI.GazeToolkit.Serialization.Csv.DataConverters
{
    public abstract class DataConverter<T> : IDataConverter
    {
        public abstract bool CanRead { get; }
        public abstract bool CanWrite { get; }

        public bool CanConvert(Type objectType)
        {
            return (objectType == typeof(T));
        }


        public void ReadCsvHeader(CsvReader reader, CsvSerializerContext serializer)
        {
            reader.ReadHeader();
        }


        public void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer)
        {
            try
            {
                foreach (var fieldName in GetHeader(serializer))
                {
                    writer.WriteField(fieldName);
                }

                writer.NextRecord();
            }
            catch (CsvHelperException exception)
            {
                throw new System.Runtime.Serialization.SerializationException("Failed to write CSV file header. See inner exception for more details.", exception);
            }
        }


        protected virtual IEnumerable<string> GetHeader(CsvSerializerContext serializer)
        {
            return Enumerable.Empty<string>();
        }


        protected virtual void WriteCsvOverride(T data, CsvWriter writer, CsvSerializerContext serializer)
        {
            throw new NotImplementedException();
        }


        public virtual void WriteCsv(object data, CsvWriter writer, CsvSerializerContext serializer)
        {
            if (data is T)
            {
                try
                {
                    WriteCsvOverride((T)data, writer, serializer);
                }
                catch (Exception exception)
                {
                    throw new SerializationException($"Failed to write or serialize next data to the CSV file. See inner exception for more details.", exception);
                }
            }
            else
            {
                throw new ArgumentException($"Type of the passed object [{data?.GetType().FullName}] does not match the type supported by this writer [{typeof(T).FullName}].");
            }
        }

        public virtual object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer)
        {
            throw new NotImplementedException();
        }
    }
}
