using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Serialization;
using UXI.GazeToolkit.Utils;
using UXI.GazeToolkit.Validation;
using UXI.Serialization.Formats.Csv;
using UXI.Serialization.Formats.Csv.Converters;

namespace UXI.GazeFilter.Validation.Serialization.Csv.Converters
{
    public class ValidationResultCsvConverter : CsvConverter<ValidationResult>
    {
        public override bool CanRead => false;


        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref ValidationResult result)
        {
            throw new NotSupportedException();
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(ValidationPoint.Validation)));
            writer.WriteField(naming.Get(nameof(ValidationPoint.Point)));

            serializer.WriteHeader<Point2>(writer, naming);
            
            serializer.WriteHeader<EyeValidationPointResult>(writer, naming, nameof(ValidationPointResult.LeftEye));
            serializer.WriteHeader<EyeValidationPointResult>(writer, naming, nameof(ValidationPointResult.RightEye));
        }


        protected override void Write(ValidationResult data, CsvWriter writer, CsvSerializerContext serializer)
        {
            bool isNextRecord = false;

            foreach (var point in data.Points)
            {
                if (isNextRecord)
                {
                    writer.NextRecord();
                }

                isNextRecord = true;

                writer.WriteField(point.Point.Validation);
                writer.WriteField(point.Point.Point);

                serializer.Serialize(writer, point.Point.Position);

                serializer.Serialize(writer, point.LeftEye);
                serializer.Serialize(writer, point.RightEye);
            }
        }
    }
}
