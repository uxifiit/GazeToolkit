using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Serialization.Json.Converters;
using UXI.GazeToolkit.Validation;

namespace UXI.GazeFilter.Validation.Serialization.Json.Converter
{
    public class DisplayAreaJsonConverter : GazeToolkit.Serialization.Json.Converters.JsonConverter<DisplayArea>
    {
        protected override DisplayArea Convert(JToken token, JsonSerializer serializer)
        {
            var obj = (JObject)token;

            var bottomLeft = obj[nameof(DisplayArea.BottomLeft)].ToObject<Point3>(serializer);
            var topLeft = obj[nameof(DisplayArea.TopLeft)].ToObject<Point3>(serializer);
            var topRight = obj[nameof(DisplayArea.TopRight)].ToObject<Point3>(serializer);

            return new DisplayArea(bottomLeft, topLeft, topRight);
        }
    }



    public class DisplayAreaChangedEventJsonConverter : GazeToolkit.Serialization.Json.Converters.JsonConverter<DisplayAreaChangedEvent>
    {
        protected override DisplayAreaChangedEvent Convert(JToken token, JsonSerializer serializer)
        {
            var obj = (JObject)token;

            var displayArea = obj.ToObject<DisplayArea>(serializer);

            var timestamp = DateTimeOffset.MinValue;
            JToken timestampToken;
            if (obj.TryGetValue("Timestamp", out timestampToken))
            {
                timestamp = timestampToken.ToObject<DateTimeOffset>(serializer);
            }

            return new DisplayAreaChangedEvent(displayArea, timestamp);
        }
    }
}
