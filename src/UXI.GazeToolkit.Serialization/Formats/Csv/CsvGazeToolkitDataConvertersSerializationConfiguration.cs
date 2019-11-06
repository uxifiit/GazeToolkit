using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Serialization.Csv.Converters;
using UXI.Serialization;
using UXI.Serialization.Formats.Csv;
using UXI.Serialization.Formats.Csv.Configurations;
using UXI.Serialization.Formats.Csv.Converters;

namespace UXI.GazeToolkit.Serialization.Csv
{
    public class CsvGazeToolkitDataConvertersSerializationConfiguration : CsvConvertersSerializationConfiguration
    {
        public CsvGazeToolkitDataConvertersSerializationConfiguration()
            : base
            (
                new Point2CsvConverter(),
                new Point3CsvConverter(),
                new EyeSampleCsvConverter(),
                new EyeDataCsvConverter(),
                new GazeDataCsvConverter(),
                new SingleEyeGazeDataCsvConverter(),
                new EyeVelocityCsvConverter(),
                new EyeMovementCsvConverter()
            )
        { }
    }
}
