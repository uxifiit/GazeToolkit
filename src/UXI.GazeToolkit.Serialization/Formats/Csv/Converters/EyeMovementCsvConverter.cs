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
    public class EyeMovementCsvConverter : CsvConverter<EyeMovement>
    {
        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<ITimestampedData>(writer, naming);

            writer.WriteField(naming.Get(nameof(EyeMovement.MovementType)));
            writer.WriteField(naming.Get(nameof(EyeMovement.Duration)));

            serializer.WriteHeader<Point2>(writer, naming, nameof(EyeMovement.Position));

            serializer.WriteHeader<Point3>(writer, naming, nameof(EyeSample.GazePoint2D));
            serializer.WriteHeader<Point3>(writer, naming, nameof(EyeSample.EyePosition3D));

            writer.WriteField(naming.Get(nameof(EyeSample.PupilDiameter)));
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var timestampedData = serializer.Deserialize<ITimestampedData>(reader, naming);

            var movementType = reader.GetField<EyeMovementType>(naming.Get(nameof(EyeMovement.MovementType)));
            var duration = reader.GetField<double>(naming.Get(nameof(EyeMovement.Duration)));

            var position = serializer.Deserialize<Point2>(reader, naming, nameof(EyeMovement.Position));

            var gazePoint3D = serializer.Deserialize<Point3>(reader, naming, nameof(EyeSample.GazePoint3D));
            var eyePosition3D = serializer.Deserialize<Point3>(reader, naming, nameof(EyeSample.EyePosition3D));

            var pupilDiameter = reader.GetField<double>(naming.Get(nameof(EyeSample.PupilDiameter)));

            var movement = new EyeMovement
            (
                movementType,
                new EyeSample
                (
                    position,
                    gazePoint3D,
                    eyePosition3D,
                    pupilDiameter
                ),
                timestampedData.Timestamp,
                timestampedData.Timestamp.AddMilliseconds(duration)
            );

            return movement;
        }


        protected override void WriteCsv(EyeMovement data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize<ITimestampedData>(writer, data);

            writer.WriteField(data.MovementType);
            writer.WriteField(data.Duration.TotalMilliseconds);

            serializer.Serialize(writer, data.Position);

            serializer.Serialize(writer, data.AverageSample?.GazePoint3D); // TODO handle nullables in CSV serialization
            serializer.Serialize(writer, data.AverageSample?.EyePosition3D);

            writer.WriteField(data.AverageSample?.PupilDiameter);
        }
    }
}
