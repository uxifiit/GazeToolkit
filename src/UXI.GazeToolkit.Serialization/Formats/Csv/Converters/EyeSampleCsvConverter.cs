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
    public class EyeSampleCsvConverter : CsvConverter<EyeSample>
    {
        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<Point2>(writer, naming, nameof(EyeSample.GazePoint2D));
            serializer.WriteHeader<Point3>(writer, naming, nameof(EyeSample.GazePoint3D));
            serializer.WriteHeader<Point3>(writer, naming, nameof(EyeSample.EyePosition3D));

            writer.WriteField(naming.Get(nameof(EyeSample.PupilDiameter)));
        }


        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref EyeSample result)
        {
            Point2 gazePoint2D;
            Point3 gazePoint3D;
            Point3 eyePosition3D;

            double pupilDiameter;

            if (
                    TryGetMember<Point2>(reader, serializer, naming, nameof(EyeData.GazePoint2D), out gazePoint2D)
                 && TryGetMember<Point3>(reader, serializer, naming, nameof(EyeData.GazePoint3D), out gazePoint3D)
                 && TryGetMember<Point3>(reader, serializer, naming, nameof(EyeData.EyePosition3D), out eyePosition3D)

                 && reader.TryGetField<double>(naming.Get(nameof(EyeData.PupilDiameter)), out pupilDiameter)
               )
            {
                result = new EyeSample
                (
                    gazePoint2D, 
                    gazePoint3D,
                    eyePosition3D, 
                    pupilDiameter
                );

                return true;
            }

            return false;
        }


        protected override void Write(EyeSample data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize(writer, data.GazePoint2D);
            serializer.Serialize(writer, data.GazePoint3D);
            serializer.Serialize(writer, data.EyePosition3D);

            writer.WriteField(data.PupilDiameter);
        }
    }
}
