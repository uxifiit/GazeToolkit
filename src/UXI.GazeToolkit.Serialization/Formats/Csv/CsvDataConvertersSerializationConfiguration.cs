using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Serialization.Csv.Converters;
using UXI.Serialization;
using UXI.Serialization.Csv;
using UXI.Serialization.Csv.Converters;

namespace UXI.GazeToolkit.Serialization.Csv
{
    public class CsvDataConvertersSerializationConfiguration : CsvConvertersSerializationConfiguration
    {
        public CsvDataConvertersSerializationConfiguration()
            : base
            (
                new EyeMovementCsvConverter(),
                new Point2CsvConverter(),
                new Point3CsvConverter()
            )
        { }
    }
}
