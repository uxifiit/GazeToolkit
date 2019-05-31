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
using UXI.Serialization.Formats.Json.Converters;
using UXI.Serialization.Formats.Json.Extensions;

namespace UXI.GazeFilter.Validation.Serialization.Json.Converter
{
    public class DisplayAreaJsonConverter : GenericJsonConverter<DisplayArea>
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



    public class DisplayAreaChangedEventJsonConverter : GenericJsonConverter<DisplayAreaChangedEvent>
    {
        protected override DisplayAreaChangedEvent Convert(JToken token, JsonSerializer serializer)
        {
            var obj = (JObject)token;

            var displayArea = obj.ToObject<DisplayArea>(serializer);

            var timestamp = obj.GetValueOrDefault<DateTimeOffset>(nameof(DisplayAreaChangedEvent.Timestamp), serializer);

            return new DisplayAreaChangedEvent(displayArea, timestamp);
        }
    }
}
