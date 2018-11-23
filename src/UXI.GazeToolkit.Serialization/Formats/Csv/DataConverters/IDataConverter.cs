using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Serialization.Csv.DataConverters
{
    public interface IDataConverter
    {
        bool CanRead { get; }
        
        bool CanWrite { get; }
        
        bool CanConvert(Type objectType);

        void ReadCsvHeader(CsvReader reader, CsvSerializerContext serializer);

        void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer);

        object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer);

        void WriteCsv(object data, CsvWriter writer, CsvSerializerContext serializer);
    }
}
