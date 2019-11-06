using Newtonsoft.Json.Linq;
using UXI.Serialization.Formats.Json.Extensions;
using UXI.Serialization.Formats.Json.Converters;
using Newtonsoft.Json;

namespace UXI.GazeToolkit.Serialization.Json.Converters
{
    public class Point3JsonConverter : GenericJsonConverter<Point3>
    {
        protected override Point3 Convert(JToken token, JsonSerializer serializer)
        {
            double x, y, z;

            x = y = z = 0d;

            if (token.Type == JTokenType.Object)
            {
                var jObject = (JObject)token;
                x = jObject.GetValueOrDefault<double>(nameof(Point3.X), serializer);
                y = jObject.GetValueOrDefault<double>(nameof(Point3.Y), serializer);
                z = jObject.GetValueOrDefault<double>(nameof(Point3.Z), serializer);
            }
            else if (token.Type == JTokenType.Array)
            {
                var array = (JArray)token;
                x = array[0].ToObject<double>(serializer);
                y = array[1].ToObject<double>(serializer);
                z = array[2].ToObject<double>(serializer);
            }

            return new Point3(x, y, z);
        }
    }
}
