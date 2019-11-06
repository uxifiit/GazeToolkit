using Newtonsoft.Json.Linq;
using UXI.Serialization.Formats.Json.Extensions;
using UXI.Serialization.Formats.Json.Converters;
using Newtonsoft.Json;

namespace UXI.GazeToolkit.Serialization.Json.Converters
{
    public class EyeSampleJsonConverter : GenericJsonConverter<EyeSample>
    {
        protected override EyeSample Convert(JToken token, JsonSerializer serializer)
        {
            JObject obj = (JObject)token;

            var gazePoint2D = obj.GetValue<Point2>(nameof(EyeSample.GazePoint2D), serializer);
            var gazePoint3D = obj.GetValue<Point3>(nameof(EyeSample.GazePoint3D), serializer);
            var eyePosition3D = obj.GetValue<Point3>(nameof(EyeSample.EyePosition3D), serializer);
            var pupilDiameter = obj.GetValue<double>(nameof(EyeSample.PupilDiameter), serializer);

            return new EyeSample
            (
                gazePoint2D,
                gazePoint3D,
                eyePosition3D,
                pupilDiameter
            );
        }
    }
}
