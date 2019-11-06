using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UXI.Serialization.Formats.Json.Extensions;
using UXI.Serialization.Formats.Json.Converters;
using Newtonsoft.Json;

namespace UXI.GazeToolkit.Serialization.Json.Converters
{
    public class EyeMovementJsonConverter : GenericJsonConverter<EyeMovement>
    {
        protected override EyeMovement Convert(JToken token, JsonSerializer serializer)
        {
            JObject obj = (JObject)token;

            var timestampedData = obj.ToObject<TimestampedData>(serializer);

            var type = obj.GetValue<EyeMovementType>(nameof(EyeMovement.MovementType), serializer);
            var samples = obj.GetValue<List<EyeVelocity>>(nameof(EyeMovement.Samples), serializer);

            var averageSample = obj.GetValue<EyeSample>(nameof(EyeMovement.AverageSample), serializer);

            var endTimestamp = obj.GetValue<DateTimeOffset>(nameof(EyeMovement.EndTimestamp), serializer); // TODO call proper time deserialize

            return new EyeMovement(type, samples, averageSample, timestampedData.Timestamp, endTimestamp);
        }
    }
}
