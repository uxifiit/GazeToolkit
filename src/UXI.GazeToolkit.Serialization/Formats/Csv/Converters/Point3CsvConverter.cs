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
        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref Point3 result)
        {
            double x;
            double y;
            double z;
            
            if (
                    reader.TryGetField<double>(naming.Get(nameof(Point3.X)), out x)
                 && reader.TryGetField<double>(naming.Get(nameof(Point3.Y)), out y)
                 && reader.TryGetField<double>(naming.Get(nameof(Point3.Z)), out z)
               )
            {
                result = new Point3(x, y, z);
                return true;
            }

            return false;
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(Point3.X)));
            writer.WriteField(naming.Get(nameof(Point3.Y)));
            writer.WriteField(naming.Get(nameof(Point3.Z)));
        }


        protected override void Write(Point3 data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.X);
            writer.WriteField(data.Y);
            writer.WriteField(data.Z);
        }
    }
}
