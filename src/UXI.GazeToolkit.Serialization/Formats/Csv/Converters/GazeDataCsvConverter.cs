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
    public class GazeDataCsvConverter : CsvConverter<GazeData>
    {
        public override bool CanRead => true;

        public override bool CanWrite => true;


        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<ITimestampedData>(writer, naming);

            serializer.WriteHeader<EyeData>(writer, naming, nameof(GazeData.LeftEye));
            serializer.WriteHeader<EyeData>(writer, naming, nameof(GazeData.RightEye));
        }


        protected override void WriteCsv(GazeData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize<ITimestampedData>(writer, data);

            serializer.Serialize<EyeData>(writer, data.LeftEye);
            serializer.Serialize<EyeData>(writer, data.RightEye);
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var timestampedData = serializer.Deserialize<ITimestampedData>(reader, naming);

            var leftEye = serializer.Deserialize<EyeData>(reader, naming, nameof(GazeData.LeftEye));
            var rightEye = serializer.Deserialize<EyeData>(reader, naming, nameof(GazeData.RightEye));

            return new GazeData(leftEye, rightEye, timestampedData.Timestamp);
        }
    }
}
