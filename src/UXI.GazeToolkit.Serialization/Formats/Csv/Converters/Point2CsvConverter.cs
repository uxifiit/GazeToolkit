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
    public class Point2CsvConverter : CsvConverter<Point2>
    {
        public override bool CanRead => true;

        public override bool CanWrite => true;


        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(Point2.X)));
            writer.WriteField(naming.Get(nameof(Point2.Y)));
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var x = reader.GetField<double>(naming.Get(nameof(Point2.X)));
            var y = reader.GetField<double>(naming.Get(nameof(Point2.Y)));

            return new Point2(x, y);
        }


        protected override void WriteCsv(Point2 data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.X);
            writer.WriteField(data.Y);
        }
    }
}
