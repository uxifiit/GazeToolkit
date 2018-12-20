using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Utils;
using UXI.GazeToolkit.Validation;
using UXI.Serialization.Csv;
using UXI.Serialization.Csv.Converters;

namespace UXI.GazeFilter.Validation.Serialization.Csv.Converters
{
    public class ValidationPointDataCsvConverter : CsvConverter<ValidationPointData>
    {
        public override bool CanRead => false;


        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            // Validation,Point,X,Y,<GazeData>
            writer.WriteField(naming.Get(nameof(ValidationPoint.Validation)));
            writer.WriteField(naming.Get(nameof(ValidationPoint.Point)));

            serializer.WriteHeader<Point2>(writer, naming);

            serializer.WriteHeader<GazeData>(writer, naming);
        }


        protected override void WriteCsv(ValidationPointData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            bool isNextRecord = false;

            foreach (var sample in data.Data)
            {
                if (isNextRecord)
                {
                    writer.NextRecord();
                }

                isNextRecord = true;

                writer.WriteField(data.Point.Validation);
                writer.WriteField(data.Point.Point);

                serializer.Serialize(writer, data.Point.Position);

                serializer.Serialize(writer, sample);
            }
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            throw new NotSupportedException();
        }
    }
}
