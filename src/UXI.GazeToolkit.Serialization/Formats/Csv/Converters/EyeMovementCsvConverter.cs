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
    public class EyeMovementCsvConverter : CsvConverter<EyeMovement>
    {
        protected override void WriteHeader(CsvWriter writer, CsvSerializerContext serializer, CsvHeaderNamingContext naming)
        {
            serializer.WriteHeader<ITimestampedData>(writer, naming);

            writer.WriteField(naming.Get(nameof(EyeMovement.MovementType)));
            writer.WriteField(naming.Get(nameof(EyeMovement.Duration)));

            serializer.WriteHeader<EyeSample>(writer, naming, "Average");
        }


        protected override bool TryRead(CsvReader reader, CsvSerializerContext serializer, CsvHeaderNamingContext naming, ref EyeMovement result)
        {
            ITimestampedData timestampedData;

            EyeMovementType movementType;
            double duration;

            EyeSample averageSample;

            if (
                    TryGetMember<ITimestampedData>(reader, serializer, naming, out timestampedData)

                 && reader.TryGetField<EyeMovementType>(naming.Get(nameof(EyeMovement.MovementType)), out movementType)
                 && reader.TryGetField<double>(naming.Get(nameof(EyeMovement.Duration)), out duration)

                 && TryGetMember<EyeSample>(reader, serializer, naming, "Average", out averageSample)
               )
            {
                result = new EyeMovement
                (
                    movementType,
                    averageSample,
                    timestampedData.Timestamp,
                    timestampedData.Timestamp.AddMilliseconds(duration)
                );

                return true;
            }

            return false;
        }


        protected override void Write(EyeMovement data, CsvWriter writer, CsvSerializerContext serializer)
        {
            serializer.Serialize<ITimestampedData>(writer, data);

            writer.WriteField(data.MovementType);
            writer.WriteField(data.Duration.TotalMilliseconds);

            serializer.Serialize<EyeSample>(writer, data.AverageSample);
        }
    }
}
