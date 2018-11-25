using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace UXI.GazeToolkit.Serialization.Csv.DataConverters
{
    public class DefaultDataConverter : IDataConverter
    {
        public virtual bool CanRead { get; } = true;

        public virtual bool CanWrite { get; } = true;

        public bool CanConvert(Type objectType)
        {
            return true;
        }

        public object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer)
        {
            return reader.GetRecord(objectType);
        }

        public void ReadCsvHeader(CsvReader reader, CsvSerializerContext serializer)
        {
            reader.ReadHeader();
        }

        public void WriteCsv(object data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteRecord(data);

            writer.NextRecord();
        }

        public void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer)
        {
            writer.WriteHeader(objectType);

            writer.NextRecord();
        }
    }
}
