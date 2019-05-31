using Newtonsoft.Json.Linq;
using UXI.Serialization.Formats.Json.Converters;
using Newtonsoft.Json;

namespace UXI.GazeToolkit.Serialization.Json.Converters
{
    public class SingleEyeGazeDataJsonConverter : GenericJsonConverter<SingleEyeGazeData>
    {
        protected override SingleEyeGazeData Convert(JToken token, JsonSerializer serializer)
        {
            JObject obj = (JObject)token;

            // SingleEyeGazeData implements the ITimestampedData interface, so deserialize a TimestampedData object and take its members
            var timestampedData = obj.ToObject<TimestampedData>(serializer);

            // SingleEyeGazeData extends the EyeData class, so we deserialize inherited class first
            var eyeGazeData = obj.ToObject<EyeData>(serializer);

            // Then we construct the SingleEyeGazeData instance from both objects
            return new SingleEyeGazeData(eyeGazeData, timestampedData.Timestamp);
        }
    }
}
