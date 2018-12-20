using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using UXI.Serialization.Csv;
using UXI.Serialization.Csv.Converters;

namespace UXI.GazeToolkit.Serialization.Csv.Converters
{
    public class EyeDataCsvConverter : CsvConverter<EyeData>
    {
        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(EyeData.Validity)));

            serializer.WriteHeader<EyeSample>(writer, naming);
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var validity = reader.GetField<EyeValidity>(naming.Get(nameof(EyeData.Validity)));

            var sample = serializer.Deserialize<EyeSample>(reader, naming);

            return new EyeData
            (
                validity,
                sample
            );
        }


        protected override void WriteCsv(EyeData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Validity);

            serializer.Serialize<EyeSample>(writer, data);
        }
    }
}
