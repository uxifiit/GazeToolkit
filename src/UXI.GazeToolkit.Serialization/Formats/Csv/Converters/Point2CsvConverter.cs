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
        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref Point2 result)
        {
            double x;
            double y;
            
            if (
                    reader.TryGetField<double>(naming.Get(nameof(Point2.X)), out x)
                 && reader.TryGetField<double>(naming.Get(nameof(Point2.Y)), out y)
               )
            {
                result = new Point2(x, y);
                return true;
            }

            return false;
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(Point2.X)));
            writer.WriteField(naming.Get(nameof(Point2.Y)));
        }


        protected override void Write(Point2 data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.X);
            writer.WriteField(data.Y);
        }
    }
}
