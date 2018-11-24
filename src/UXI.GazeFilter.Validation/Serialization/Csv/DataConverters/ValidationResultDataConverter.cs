using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Serialization;
using UXI.GazeToolkit.Serialization.Csv;
using UXI.GazeToolkit.Serialization.Csv.DataConverters;
using UXI.GazeToolkit.Utils;
using UXI.GazeToolkit.Validation;

namespace UXI.GazeFilter.Validation.Serialization.Csv.DataConverters
{
    public class ValidationResultDataConverter : DataConverter<ValidationResult>
    {
        public override bool CanRead => false;


        public override bool CanWrite => true;


        protected override IEnumerable<string> GetHeader(CsvSerializerContext serializer)
        {
            return new[]
            {
                     "Validation", "Point", "X", "Y"
                  , $"{nameof(ValidationPointResult.LeftEye)}{nameof(EyeValidationPointResult.PrecisionRMS)}"
                  , $"{nameof(ValidationPointResult.LeftEye)}{nameof(EyeValidationPointResult.PrecisionSD)}"
                  , $"{nameof(ValidationPointResult.RightEye)}{nameof(EyeValidationPointResult.PrecisionRMS)}"
                  , $"{nameof(ValidationPointResult.RightEye)}{nameof(EyeValidationPointResult.PrecisionSD)}"
                  , $"{nameof(ValidationPointResult.LeftEye)}{nameof(EyeValidationPointResult.Accuracy)}"
                  , $"{nameof(ValidationPointResult.RightEye)}{nameof(EyeValidationPointResult.Accuracy)}"
                  , $"{nameof(ValidationPointResult.LeftEye)}{nameof(EyeValidationPointResult.ValidRatio)}"
                  , $"{nameof(ValidationPointResult.RightEye)}{nameof(EyeValidationPointResult.ValidRatio)}"
                  , $"{nameof(ValidationPointResult.LeftEye)}{nameof(EyeValidationPointResult.Distance)}"
                  , $"{nameof(ValidationPointResult.RightEye)}{nameof(EyeValidationPointResult.Distance)}"
            };
        }


        protected override void WriteCsvOverride(ValidationResult data, CsvWriter writer, CsvSerializerContext serializer)
        {
            int pointId = 1;
            foreach (var point in data.Points)
            {
                writer.WriteField(data.Id);
                writer.WriteField(pointId);
                writer.WriteField(point.TargetPoint2D.X);
                writer.WriteField(point.TargetPoint2D.Y);
                writer.WriteField(point.LeftEye.PrecisionRMS);
                writer.WriteField(point.LeftEye.PrecisionSD);
                writer.WriteField(point.RightEye.PrecisionRMS);
                writer.WriteField(point.RightEye.PrecisionSD);
                writer.WriteField(point.LeftEye.Accuracy);
                writer.WriteField(point.RightEye.Accuracy);
                writer.WriteField(point.LeftEye.ValidRatio);
                writer.WriteField(point.RightEye.ValidRatio);
                writer.WriteField(point.LeftEye.Distance);
                writer.WriteField(point.RightEye.Distance);

                writer.NextRecord();
                pointId += 1;
            }
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer)
        {
            throw new InvalidOperationException();
        }
    }
}
