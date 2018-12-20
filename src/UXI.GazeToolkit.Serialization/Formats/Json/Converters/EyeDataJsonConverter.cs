using Newtonsoft.Json.Linq;
using UXI.Serialization.Json.Extensions;
using UXI.Serialization.Json.Converters;
using Newtonsoft.Json;

namespace UXI.GazeToolkit.Serialization.Json.Converters
{
    public class EyeDataJsonConverter : GenericJsonConverter<EyeData>
    {
        protected override EyeData Convert(JToken token, JsonSerializer serializer)
        {
            JObject obj = (JObject)token;

            var sample = obj.ToObject<EyeSample>(serializer);

            var validity = obj.GetValue<EyeValidity>(nameof(EyeData.Validity), serializer);

            return new EyeData
            (
                validity,
                sample
            );
        }
    }
}
