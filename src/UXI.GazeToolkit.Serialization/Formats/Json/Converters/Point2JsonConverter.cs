using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Serialization.Json.Converters;
using UXI.Serialization.Json.Extensions;

namespace UXI.GazeToolkit.Serialization.Json.Converters
{
    public class Point2Converter : GenericJsonConverter<Point2>
    {
        protected override Point2 Convert(JToken token, JsonSerializer serializer)
        {
            double x, y;

            x = y = 0d;

            if (token.Type == JTokenType.Object)
            {
                var jObject = (JObject)token;
                x = jObject.GetValueOrDefault<double>(nameof(Point2.X), serializer);
                y = jObject.GetValueOrDefault<double>(nameof(Point2.Y), serializer);
            }
            else if (token.Type == JTokenType.Array)
            {
                var array = (JArray)token;

                x = array[0].ToObject<double>(serializer);
                y = array[1].ToObject<double>(serializer);
            }

            return new Point2(x, y);
        }
    }
}
