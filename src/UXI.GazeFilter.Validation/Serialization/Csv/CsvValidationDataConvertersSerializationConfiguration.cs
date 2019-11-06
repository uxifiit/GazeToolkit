using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeFilter.Validation.Serialization.Csv.Converters;
using UXI.Serialization;
using UXI.Serialization.Formats.Csv;
using UXI.Serialization.Formats.Csv.Configurations;
using UXI.Serialization.Formats.Csv.Converters;

namespace UXI.GazeToolkit.Validation.Serialization.Csv
{
    public class CsvValidationDataConvertersSerializationConfiguration : CsvConvertersSerializationConfiguration
    {
        public CsvValidationDataConvertersSerializationConfiguration()
            : base
            (
                new EyeValidationPointResultCsvConverter(),
                new ValidationPointCsvConverter(),
                new ValidationPointDataCsvConverter(),
                new ValidationResultCsvConverter()
            )
        { }
    }
}
