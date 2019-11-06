using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using UXI.Serialization.Formats.Csv;
using UXI.Serialization.Formats.Csv.Converters;

namespace UXI.GazeToolkit.Serialization.Csv.Converters
{
    public class GazeDataCsvConverter : CsvConverter<GazeData>
    {
        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref GazeData result)
        {
            ITimestampedData timestampedData;
            EyeData leftEye;
            EyeData rightEye;
            
            if (
                    TryGetMember<ITimestampedData>(reader, serializer, naming, out timestampedData)
                 && TryGetMember<EyeData>(reader, serializer, naming, "Left",  out leftEye)
                 && TryGetMember<EyeData>(reader, serializer, naming, "Right", out rightEye)
               )
            {
                result = new GazeData(leftEye, rightEye, timestampedData.Timestamp);
                return true;
            }

            return false;
        }


        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<ITimestampedData>(writer, naming);

            serializer.WriteHeader<EyeData>(writer, naming, "Left");
            serializer.WriteHeader<EyeData>(writer, naming, "Right");
        }


        protected override void Write(GazeData data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize<ITimestampedData>(writer, data);

            serializer.Serialize<EyeData>(writer, data.LeftEye);
            serializer.Serialize<EyeData>(writer, data.RightEye);
        }
    }
}
