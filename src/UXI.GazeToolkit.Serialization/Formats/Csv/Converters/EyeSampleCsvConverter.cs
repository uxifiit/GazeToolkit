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
        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<Point2>(writer, naming, nameof(EyeSample.GazePoint2D));
            serializer.WriteHeader<Point3>(writer, naming, nameof(EyeSample.GazePoint3D));
            serializer.WriteHeader<Point3>(writer, naming, nameof(EyeSample.EyePosition3D));

            writer.WriteField(naming.Get(nameof(EyeSample.PupilDiameter)));
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var gazePoint2D = serializer.Deserialize<Point2>(reader, naming, nameof(EyeData.GazePoint2D));
            var gazePoint3D = serializer.Deserialize<Point3>(reader, naming, nameof(EyeData.GazePoint3D));
            var eyePosition3D = serializer.Deserialize<Point3>(reader, naming, nameof(EyeData.EyePosition3D));

            var pupilDiameter = reader.GetField<double>(naming.Get(nameof(EyeData.PupilDiameter)));

            return new EyeSample
            (
                gazePoint2D, 
                gazePoint3D,
                eyePosition3D, 
                pupilDiameter
            );
        }


        protected override void WriteCsv(EyeSample data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize(writer, data.GazePoint2D);
            serializer.Serialize(writer, data.GazePoint3D);
            serializer.Serialize(writer, data.EyePosition3D);

            writer.WriteField(data.PupilDiameter);
        }
    }
}
