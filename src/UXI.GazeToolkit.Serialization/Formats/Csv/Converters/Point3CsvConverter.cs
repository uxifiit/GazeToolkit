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
    public class Point3CsvConverter : CsvConverter<Point3>
    {
        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(Point3.X)));
            writer.WriteField(naming.Get(nameof(Point3.Y)));
            writer.WriteField(naming.Get(nameof(Point3.Z)));
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var x = reader.GetField<double>(naming.Get(nameof(Point3.X)));
            var y = reader.GetField<double>(naming.Get(nameof(Point3.Y)));
            var z = reader.GetField<double>(naming.Get(nameof(Point3.Z)));

            return new Point3(x, y, z);
        }


        protected override void WriteCsv(Point3 data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.X);
            writer.WriteField(data.Y);
            writer.WriteField(data.Z);
        }
    }
}
