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
        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(EyeData.Validity)));

            serializer.WriteHeader<EyeSample>(writer, naming);
        }


        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref EyeData result)
        {
            EyeValidity validity;
            EyeSample sample;

            if (
                    reader.TryGetField<EyeValidity>(naming.Get(nameof(EyeData.Validity)), out validity)
                 && TryGetMember<EyeSample>(reader, serializer, naming, out sample)
               )
            {
                result = new EyeData
                (
                    validity,
                    sample
                );
                return true;
            }

            return false;
        }


        protected override void Write(EyeData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Validity);

            serializer.Serialize<EyeSample>(writer, data);
        }
    }
}
