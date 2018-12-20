using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using UXI.GazeToolkit.Validation;
using UXI.Serialization.Csv;
using UXI.Serialization.Csv.Converters;

namespace UXI.GazeFilter.Validation.Serialization.Csv.DataConverters
{
    class EyeValidationPointResultCsvConverter : CsvConverter<EyeValidationPointResult>
    {
        public override bool CanRead => false;


        public override bool CanWrite => true;


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            throw new NotSupportedException();
        }


        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(EyeValidationPointResult.PrecisionRMS)));
            writer.WriteField(naming.Get(nameof(EyeValidationPointResult.PrecisionSD)));
            writer.WriteField(naming.Get(nameof(EyeValidationPointResult.Accuracy)));
            writer.WriteField(naming.Get(nameof(EyeValidationPointResult.ValidRatio)));
            writer.WriteField(naming.Get(nameof(EyeValidationPointResult.Distance)));
        }


        protected override void WriteCsv(EyeValidationPointResult data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.PrecisionRMS);
            writer.WriteField(data.PrecisionSD);
            writer.WriteField(data.Accuracy);
            writer.WriteField(data.ValidRatio);
            writer.WriteField(data.Distance);
        }
    }
}
