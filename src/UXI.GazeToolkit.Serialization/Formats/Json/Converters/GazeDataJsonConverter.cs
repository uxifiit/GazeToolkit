using Newtonsoft.Json.Linq;
using UXI.Serialization.Formats.Json.Extensions;
using UXI.Serialization.Formats.Json.Converters;
using Newtonsoft.Json;

namespace UXI.GazeToolkit.Serialization.Json.Converters
{
    public class GazeDataJsonConverter : GenericJsonConverter<GazeData>
    {
        protected override GazeData Convert(JToken token, JsonSerializer serializer)
        {
            JObject obj = (JObject)token;

            // GazeData implements the ITimestampedData interface, so deserialize a TimestampedData object and take its members
            var timestampedData = obj.ToObject<TimestampedData>(serializer);

            var leftEye = obj.GetValue<EyeData>(nameof(GazeData.LeftEye), serializer);
            var rightEye = obj.GetValue<EyeData>(nameof(GazeData.RightEye), serializer);

            return new GazeData(leftEye, rightEye, timestampedData.Timestamp);
        }
    }
}
