using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Serialization.Csv;
using UXI.GazeToolkit.Serialization.Csv.DataConverters;
using UXI.GazeToolkit.Utils;
using UXI.GazeToolkit.Validation;

namespace UXI.GazeFilter.Validation.Serialization.Csv.DataConverters
{
    public class ValidationPointGazeDataConverter : DataConverter<ValidationPointGaze>
    {
        private int _pointId = 0;
        private int _lastValidationId = 0;


        public override bool CanRead => false;

        public override bool CanWrite => true;


        protected override IEnumerable<string> GetHeader(CsvSerializerContext serializer)
        {
            return new[]
            {
                nameof(ValidationPoint.Validation)
              , nameof(ValidationPoint.Point)
              , nameof(Point2.X)
              , nameof(Point2.Y)
              , "Timestamp"
              , "LeftValidity", "LeftGazePointX", "LeftGazePointY", "LeftDistance"
              , "RightValidity", "RightGazePointX", "RightGazePointY", "RightDistance"
            };
        }


        protected override void WriteCsvOverride(ValidationPointGaze data, CsvWriter writer, CsvSerializerContext serializer)
        {
            if (_lastValidationId != data.Point.Validation)
            {
                _lastValidationId = data.Point.Validation;
                _pointId = 1;
            }
            else
            {
                _pointId += 1;
            }

            foreach (var sample in data.Gaze)
            {
                writer.WriteField(data.Point.Validation);
                writer.WriteField(_pointId);
                writer.WriteField(data.Point.Point.X);
                writer.WriteField(data.Point.Point.Y);

                writer.WriteField((sample.Timestamp - data.Point.StartTime).TotalMilliseconds);

                writer.WriteField(sample.LeftEye.Validity);
                writer.WriteField(sample.LeftEye.GazePoint2D.X);
                writer.WriteField(sample.LeftEye.GazePoint2D.Y);
                writer.WriteField(PointUtils.Vectors.GetLength(PointUtils.Vectors.GetVector(sample.LeftEye.EyePosition3D, sample.LeftEye.GazePoint3D)));

                writer.WriteField(sample.RightEye.Validity);
                writer.WriteField(sample.RightEye.GazePoint2D.X);
                writer.WriteField(sample.RightEye.GazePoint2D.Y);
                writer.WriteField(PointUtils.Vectors.GetLength(PointUtils.Vectors.GetVector(sample.RightEye.EyePosition3D, sample.RightEye.GazePoint3D)));

                writer.NextRecord();
            }
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer)
        {
            throw new InvalidOperationException();
        }
    }
}
