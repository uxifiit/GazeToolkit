using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Validation;

namespace UXI.GazeFilter.Validation.Serialization.Json.Converter
{
    public class ValidationPointJsonConverter : GazeToolkit.Serialization.Json.Converters.JsonConverter<ValidationPoint>
    {
        protected override ValidationPoint Convert(JToken token, JsonSerializer serializer)
        {
            var obj = (JObject)token;

            var validation = obj[nameof(ValidationPoint.Validation)].ToObject<int>(serializer);
            var point = obj.ToObject<Point2>(serializer);
            var startTime = obj[nameof(ValidationPoint.StartTime)].ToObject<DateTimeOffset>(serializer);
            var endTime = obj[nameof(ValidationPoint.EndTime)].ToObject<DateTimeOffset>(serializer);

            return new ValidationPoint(validation, point, startTime, endTime);
        }
    }
}
