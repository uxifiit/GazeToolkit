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
        public override bool CanWrite => false;


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            // Validation,Point,X,Y,StartTime,EndTime
            int validation = reader.GetField<int>(naming.Get(nameof(ValidationPoint.Validation)));
            int point = reader.GetField<int>(naming.Get(nameof(ValidationPoint.Point)));

            Point2 position = serializer.Deserialize<Point2>(reader, naming);

            DateTimeOffset startTime = reader.GetField<DateTimeOffset>(naming.Get(nameof(ValidationPoint.StartTime)));
            DateTimeOffset endTime = reader.GetField<DateTimeOffset>(naming.Get(nameof(ValidationPoint.EndTime)));

            return new ValidationPoint(validation, point, position, startTime, endTime);
        }


        protected override void WriteCsv(ValidationPoint data, CsvWriter writer, CsvSerializerContext serializer)
        {
            throw new NotSupportedException();
        }


        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            throw new NotSupportedException();
        }
    }
}
