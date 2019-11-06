using Newtonsoft.Json.Linq;
using UXI.Serialization.Formats.Json.Extensions;
using UXI.Serialization.Formats.Json.Converters;
using Newtonsoft.Json;

namespace UXI.GazeToolkit.Serialization.Json.Converters
{
    public class EyeVelocityJsonConverter : GenericJsonConverter<EyeVelocity>
    {
        protected override EyeVelocity Convert(JToken token, JsonSerializer serializer)
        {
            JObject obj = (JObject)token;

            var velocity = obj.GetValue<double>(nameof(EyeVelocity.Velocity), serializer);
            var eye = obj.GetValue<SingleEyeGazeData>(nameof(EyeVelocity.Eye), serializer);

            return new EyeVelocity(velocity, eye);
        }
    }
}
