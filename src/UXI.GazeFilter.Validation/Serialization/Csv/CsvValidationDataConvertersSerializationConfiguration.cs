using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeFilter.Validation.Serialization.Csv.DataConverters;
using UXI.Serialization;
using UXI.Serialization.Csv;
using UXI.Serialization.Csv.Converters;

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
