using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using UXI.Serialization.Formats.Csv;
using UXI.Serialization.Formats.Csv.Converters;

namespace UXI.GazeToolkit.Serialization.Csv.Converters
{
    public class EyeVelocityCsvConverter : CsvConverter<EyeVelocity>
    {
        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref EyeVelocity result)
        {
            double velocity;
            SingleEyeGazeData eye;
            
            if (
                    reader.TryGetField<double>(naming.Get(nameof(EyeVelocity.Velocity)), out velocity)
                 && TryGetMember<SingleEyeGazeData>(reader, serializer, naming, out eye)
               )
            {
                result = new EyeVelocity(velocity, eye);
                return true;
            }

            return false;
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(EyeVelocity.Velocity)));

            serializer.WriteHeader<SingleEyeGazeData>(writer, naming);
        }


        protected override void Write(EyeVelocity data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Velocity);

            serializer.Serialize<SingleEyeGazeData>(writer, data.Eye);
        }
    }
}
