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
    public class SingleEyeGazeDataCsvConverter : CsvConverter<SingleEyeGazeData>
    {
        public override void WriteCsvHeader(CsvWriter writer, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<ITimestampedData>(writer, naming);

            serializer.WriteHeader<EyeData>(writer, naming);
        }


        public override object ReadCsv(CsvReader reader, Type objectType, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            var timestampedData = serializer.Deserialize<ITimestampedData>(reader, naming);

            var eyeGazeData = serializer.Deserialize<EyeData>(reader, naming);

            return new SingleEyeGazeData(eyeGazeData, timestampedData.Timestamp);
        }


        protected override void WriteCsv(SingleEyeGazeData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize<ITimestampedData>(writer, data); // TODO needs to be ITimestampedData

            serializer.Serialize<EyeData>(writer, data);
        }
    }
}
