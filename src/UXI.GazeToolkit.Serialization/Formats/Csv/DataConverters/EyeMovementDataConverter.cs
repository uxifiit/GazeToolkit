using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace UXI.GazeToolkit.Serialization.Csv.DataConverters
{
    class EyeMovementDataConverter : DataConverter<EyeMovement>
    {
        public override bool CanRead => true;

        public override bool CanWrite => true;

        protected override IEnumerable<string> GetHeader(CsvSerializerContext serializer)
        {
            return new string[] 
            {
                nameof(EyeMovement.Timestamp)
              , nameof(EyeMovement.MovementType)
              , nameof(EyeMovement.Duration)
              , $"{nameof(EyeMovement.Position)}{nameof(Point2.X)}"
              , $"{nameof(EyeMovement.Position)}{nameof(Point2.Y)}"
              , $"{nameof(EyeSample.GazePoint3D)}{nameof(Point3.X)}"
              , $"{nameof(EyeSample.GazePoint3D)}{nameof(Point3.Y)}"
              , $"{nameof(EyeSample.GazePoint3D)}{nameof(Point3.Z)}"
              , $"{nameof(EyeSample.EyePosition3D)}{nameof(Point3.X)}"
              , $"{nameof(EyeSample.EyePosition3D)}{nameof(Point3.Y)}"
              , $"{nameof(EyeSample.EyePosition3D)}{nameof(Point3.Z)}"
              , $"{nameof(EyeSample.PupilDiameter)}"
            };
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer)
        {
            var timestamp = reader.GetField<DateTimeOffset>(nameof(EyeMovement.Timestamp));
            var movementType = reader.GetField<EyeMovementType>(nameof(EyeMovement.MovementType));
            var duration = reader.GetField<double>(nameof(EyeMovement.Duration));
            var positionX = reader.GetField<double>($"{nameof(EyeMovement.Position)}{nameof(Point2.X)}");
            var positionY = reader.GetField<double>($"{nameof(EyeMovement.Position)}{nameof(Point2.Y)}");
            var gazePoint3DX = reader.GetField<double>($"{nameof(EyeSample.GazePoint3D)}{nameof(Point3.X)}");
            var gazePoint3DY = reader.GetField<double>($"{nameof(EyeSample.GazePoint3D)}{nameof(Point3.Y)}");
            var gazePoint3DZ = reader.GetField<double>($"{nameof(EyeSample.GazePoint3D)}{nameof(Point3.Z)}");
            var eyePosition3DX = reader.GetField<double>($"{nameof(EyeSample.EyePosition3D)}{nameof(Point3.X)}");
            var eyePosition3DY = reader.GetField<double>($"{nameof(EyeSample.EyePosition3D)}{nameof(Point3.Y)}");
            var eyePosition3DZ = reader.GetField<double>($"{nameof(EyeSample.EyePosition3D)}{nameof(Point3.Z)}");
            var pupilDiameter = reader.GetField<double>($"{nameof(EyeSample.PupilDiameter)}");

            var movement = new EyeMovement
            (
                movementType,
                new EyeSample
                (
                    new Point2(positionX, positionY),
                    new Point3(gazePoint3DX, gazePoint3DY, gazePoint3DZ),
                    new Point3(eyePosition3DX, eyePosition3DY, eyePosition3DZ),
                    pupilDiameter
                ),
                timestamp,
                timestamp.AddMilliseconds(duration)
            );

            return movement;
        }


        protected override void WriteCsvOverride(EyeMovement data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Timestamp);
            writer.WriteField(data.MovementType);
            writer.WriteField(data.Duration.TotalMilliseconds);
            writer.WriteField(data.Position.X);
            writer.WriteField(data.Position.Y);
            writer.WriteField(data.AverageSample?.GazePoint3D.X);
            writer.WriteField(data.AverageSample?.GazePoint3D.Y);
            writer.WriteField(data.AverageSample?.GazePoint3D.Z);
            writer.WriteField(data.AverageSample?.EyePosition3D.X);
            writer.WriteField(data.AverageSample?.EyePosition3D.Y);
            writer.WriteField(data.AverageSample?.EyePosition3D.Z);
            writer.WriteField(data.AverageSample?.PupilDiameter);

            writer.NextRecord();
        }
    }
}
