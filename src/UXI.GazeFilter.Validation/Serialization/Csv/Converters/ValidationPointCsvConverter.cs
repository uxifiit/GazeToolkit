using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Validation;
using UXI.Serialization.Csv;
using UXI.Serialization.Csv.Converters;

namespace UXI.GazeFilter.Validation.Serialization.Csv.Converters
{
    public class ValidationPointCsvConverter : CsvConverter<ValidationPoint>
    {
        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref ValidationPoint result)
        {
            // Validation,Point,X,Y,StartTime,EndTime
            int validation;
            int point;
            
            Point2 position;

            DateTimeOffset startTime;
            DateTimeOffset endTime;
            
            if (
                    reader.TryGetField<int>(naming.Get(nameof(ValidationPoint.Validation)), out validation)
                 && reader.TryGetField<int>(naming.Get(nameof(ValidationPoint.Point)), out point)
                    
                 && TryGetMember<Point2>(reader, serializer, naming, out position)

                 && reader.TryGetField<DateTimeOffset>(naming.Get(nameof(ValidationPoint.StartTime)), out startTime)
                 && reader.TryGetField<DateTimeOffset>(naming.Get(nameof(ValidationPoint.EndTime)), out endTime)
               )
            {
                result = new ValidationPoint(validation, point, position, startTime, endTime);

                return true;
            }

            return false;
        }


        protected override void Write(ValidationPoint data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Validation);
            writer.WriteField(data.Point);

            serializer.Serialize(writer, data.Point);

            writer.WriteField(data.StartTime);
            writer.WriteField(data.EndTime);
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(ValidationPoint.Validation)));
            writer.WriteField(naming.Get(nameof(ValidationPoint.Point)));

            serializer.WriteHeader<Point2>(writer, naming);

            writer.WriteField(naming.Get(nameof(ValidationPoint.StartTime)));
            writer.WriteField(naming.Get(nameof(ValidationPoint.EndTime)));
        }
    }
}
