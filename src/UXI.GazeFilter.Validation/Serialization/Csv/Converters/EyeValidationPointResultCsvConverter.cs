using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using UXI.GazeToolkit.Validation;
using UXI.Serialization.Csv;
using UXI.Serialization.Csv.Converters;

namespace UXI.GazeFilter.Validation.Serialization.Csv.Converters
{
    class EyeValidationPointResultCsvConverter : CsvConverter<EyeValidationPointResult>
    {
        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref EyeValidationPointResult result)
        {
            double? accuracy;
            double? precisionSD;
            double? precisionRMS;
            double validRatio;
            double? distance;
            double? pupilDiameter;

            if (
                   reader.TryGetField<double?>(naming.Get(nameof(EyeValidationPointResult.Accuracy)), out accuracy)
                && reader.TryGetField<double?>(naming.Get(nameof(EyeValidationPointResult.PrecisionSD)), out precisionSD)
                && reader.TryGetField<double?>(naming.Get(nameof(EyeValidationPointResult.PrecisionRMS)), out precisionRMS)
                && reader.TryGetField<double>(naming.Get(nameof(EyeValidationPointResult.ValidRatio)), out validRatio)
                && reader.TryGetField<double?>(naming.Get(nameof(EyeValidationPointResult.Distance)), out distance)
                && reader.TryGetField<double?>(naming.Get(nameof(EyeValidationPointResult.PupilDiameter)), out pupilDiameter)
               )
            {
                result = new EyeValidationPointResult
                (
                    accuracy,
                    precisionSD,
                    precisionRMS,
                    validRatio,
                    distance,
                    pupilDiameter
                );
                return true;
            }

            return false;
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            writer.WriteField(naming.Get(nameof(EyeValidationPointResult.Accuracy)));
            writer.WriteField(naming.Get(nameof(EyeValidationPointResult.PrecisionSD)));
            writer.WriteField(naming.Get(nameof(EyeValidationPointResult.PrecisionRMS)));
            writer.WriteField(naming.Get(nameof(EyeValidationPointResult.ValidRatio)));
            writer.WriteField(naming.Get(nameof(EyeValidationPointResult.Distance)));
            writer.WriteField(naming.Get(nameof(EyeValidationPointResult.PupilDiameter)));
        }


        protected override void Write(EyeValidationPointResult data, CsvWriter writer, CsvSerializerContext serializer)
        {
            writer.WriteField(data.Accuracy);
            writer.WriteField(data.PrecisionSD);
            writer.WriteField(data.PrecisionRMS);
            writer.WriteField(data.ValidRatio);
            writer.WriteField(data.Distance);
            writer.WriteField(data.PupilDiameter);
        }
    }
}
