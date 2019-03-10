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
        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref SingleEyeGazeData result)
        {
            ITimestampedData timestampedData;
            EyeData eyeGazeData;

            if (
                    TryGetMember<ITimestampedData>(reader, serializer, naming, out timestampedData)
                 && TryGetMember<EyeData>(reader, serializer, naming, out eyeGazeData)
               )
            {
                result = new SingleEyeGazeData(eyeGazeData, timestampedData.Timestamp);
                return true;
            }

            return false;
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<ITimestampedData>(writer, naming);

            serializer.WriteHeader<EyeData>(writer, naming);
        }


        protected override void Write(SingleEyeGazeData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize<ITimestampedData>(writer, data);

            serializer.Serialize<EyeData>(writer, data);
        }
    }
}
