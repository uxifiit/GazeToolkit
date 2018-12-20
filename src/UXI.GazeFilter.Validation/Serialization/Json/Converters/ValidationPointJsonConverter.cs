using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Validation;
using UXI.Serialization.Json.Converters;
using UXI.Serialization.Json.Extensions;

namespace UXI.GazeFilter.Validation.Serialization.Json.Converter
{
    public class ValidationPointJsonConverter : GenericJsonConverter<ValidationPoint>
    {
        protected override ValidationPoint Convert(JToken token, JsonSerializer serializer)
        {
            var obj = (JObject)token;

            var validation = obj.GetValueOrDefault<int>(nameof(ValidationPoint.Validation), serializer);
            var point = obj.GetValueOrDefault<int>(nameof(ValidationPoint.Point), serializer);

            var position = obj.ToObject<Point2>(serializer);

            var startTime = obj.GetValueOrDefault<DateTimeOffset>(nameof(ValidationPoint.StartTime), serializer);
            var endTime = obj.GetValueOrDefault<DateTimeOffset>(nameof(ValidationPoint.EndTime), serializer);

            return new ValidationPoint(validation, point, position, startTime, endTime);
        }
    }
}
