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
    public class EyeVelocityCsvConverter : CsvConverter<EyeVelocity>
    {
        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(EyeVelocity.Velocity)));

            serializer.WriteHeader<SingleEyeGazeData>(writer, naming);
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var velocity = reader.GetField<double>(naming.Get(nameof(EyeVelocity.Velocity)));

            var eye = serializer.Deserialize<SingleEyeGazeData>(reader, naming);

            return new EyeVelocity(velocity, eye);
        }


        protected override void WriteCsv(EyeVelocity data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Velocity);

            serializer.Serialize<SingleEyeGazeData>(writer, data.Eye);
        }
    }
}
