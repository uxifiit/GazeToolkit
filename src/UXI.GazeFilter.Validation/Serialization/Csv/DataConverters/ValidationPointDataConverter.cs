using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Serialization.Csv;
using UXI.GazeToolkit.Serialization.Csv.DataConverters;
using UXI.GazeToolkit.Validation;

namespace UXI.GazeFilter.Validation.Serialization.Csv.DataConverters
{
    public class ValidationPointDataConverter : DataConverter<ValidationPoint>
    {
        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer)
        {
            int validation = reader.GetField<int>(nameof(ValidationPoint.Validation));
            double x = reader.GetField<double>(nameof(Point2.X));
            double y = reader.GetField<double>(nameof(Point2.Y));

            DateTimeOffset startTime = reader.GetField<DateTimeOffset>(nameof(ValidationPoint.StartTime));
            DateTimeOffset endTime = reader.GetField<DateTimeOffset>(nameof(ValidationPoint.EndTime));

            return new ValidationPoint(validation, new Point2(x, y), startTime, endTime);
        }
    }
}
