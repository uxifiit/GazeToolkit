using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UXI.GazeToolkit.Serialization.Json.Extensions;
using UXI.GazeToolkit.Serialization.Converters;

namespace UXI.GazeToolkit.Serialization.Json.Converters
{
    public class DataJsonConverters : IEnumerable<JsonConverter>
    {
        public static readonly IEnumerable<JsonConverter> Converters = new List<JsonConverter>()
        {
            new Point2Converter(),
            new Point3Converter(),
            new EyeSampleConverter(),
            new EyeDataConverter(),
            new GazeDataConverter(),
            new SingleEyeGazeDataJsonConverter(),
            new EyeVelocityJsonConverter(),
            new EyeMovementJsonConverter()
        };

        public IEnumerator<JsonConverter> GetEnumerator()
        {
            return Converters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }



    public class Point2Converter : JsonConverter<Point2>
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



    class Point3Converter : JsonConverter<Point3>
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



    class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
    {
        private readonly ITimestampStringConverter _timestampConverter;

        public DateTimeOffsetJsonConverter(ITimestampStringConverter timestampConverter)
        {
            _timestampConverter = timestampConverter;
        }

        public override bool CanWrite => true;

        protected override DateTimeOffset Convert(JToken token, JsonSerializer serializer)
        {
            return _timestampConverter.Convert(token.Value<string>());
        }

        protected override JToken ConvertBack(DateTimeOffset value, JsonSerializer serializer)
        {
            return _timestampConverter.ConvertBack(value);
        }
    }



    class TimestampedDataJsonConverter : JsonConverter<ITimestampedData>
    {
        private readonly string _timestampFieldName;


        public TimestampedDataJsonConverter()
        {
            _timestampFieldName = nameof(TimestampedData.Timestamp);
        }


        public TimestampedDataJsonConverter(string timestampFieldName)
        {
            string fieldName = timestampFieldName?.Trim();
            if (String.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentException("Timestamp field name cannot be null, empty or white space only.", nameof(timestampFieldName));
            }

            _timestampFieldName = fieldName;
        }


        public override bool CanConvert(Type objectType)
        {
            return base.CanConvert(objectType) || objectType == typeof(TimestampedData);
        }


        protected override ITimestampedData Convert(JToken token, JsonSerializer serializer)
        {
            JObject obj = (JObject)token;

            var timestamp = obj.GetValue<DateTimeOffset>(_timestampFieldName, serializer);

            return new TimestampedData(timestamp);
        }
    }



    class EyeSampleConverter : JsonConverter<EyeSample>
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



    class EyeDataConverter : JsonConverter<EyeData>
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



    class GazeDataConverter : JsonConverter<GazeData>
    {
        protected override GazeData Convert(JToken token, JsonSerializer serializer)
        {
            JObject obj = (JObject)token;

            // GazeData implements the ITimestampedData interface, so deserialize a TimestampedData object and take its members
            var timestampedData = obj.ToObject<TimestampedData>(serializer);

            var leftEye = obj.GetValue<EyeData>(nameof(GazeData.LeftEye), serializer);
            var rightEye = obj.GetValue<EyeData>(nameof(GazeData.RightEye), serializer);

            return new GazeData(leftEye, rightEye, timestampedData.Timestamp);
        }
    }



    class SingleEyeGazeDataJsonConverter : JsonConverter<SingleEyeGazeData>
    {
        protected override SingleEyeGazeData Convert(JToken token, JsonSerializer serializer)
        {
            JObject obj = (JObject)token;

            // SingleEyeGazeData extends the EyeData class, so we deserialize inherited class first
            var eyeGazeData = obj.ToObject<EyeData>(serializer);

            // Then we take other members of SingleEyeGazeData

            // SingleEyeGazeData implements the ITimestampedData interface, so deserialize a TimestampedData object and take its members
            var timestampedData = obj.ToObject<TimestampedData>(serializer);

            // Then we construct the SingleEyeGazeData instance from both objects
            return new SingleEyeGazeData(eyeGazeData, timestampedData.Timestamp);
        }
    }



    class EyeVelocityJsonConverter : JsonConverter<EyeVelocity>
    {
        protected override EyeVelocity Convert(JToken token, JsonSerializer serializer)
        {
            JObject obj = (JObject)token;

            var velocity = obj.GetValue<double>(nameof(EyeVelocity.Velocity), serializer);
            var eye = obj.GetValue<SingleEyeGazeData>(nameof(EyeVelocity.Eye), serializer);

            return new EyeVelocity(velocity, eye);
        }
    }



    class EyeMovementJsonConverter : JsonConverter<EyeMovement>
    {
        protected override EyeMovement Convert(JToken token, JsonSerializer serializer)
        {
            JObject obj = (JObject)token;

            var timestampedData = obj.ToObject<TimestampedData>(serializer);

            var type = obj.GetValue<EyeMovementType>(nameof(EyeMovement.MovementType), serializer);
            var samples = obj.GetValue<List<EyeVelocity>>(nameof(EyeMovement.Samples), serializer);

            var averageSample = obj.GetValue<EyeSample>(nameof(EyeMovement.AverageSample), serializer);

            var endTimestamp = obj.GetValue<DateTimeOffset>(nameof(EyeMovement.EndTimestamp), serializer); // TODO call proper time deserialize

            return new EyeMovement(type, samples, averageSample, timestampedData.Timestamp, endTimestamp);
        }
    }
}
