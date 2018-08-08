using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UXI.GazeToolkit;
using UXI.GazeFilter.Serialization.Json.Extensions;

namespace UXI.GazeFilter.Serialization.Json.Converters
{
    public class DataJsonConverters : IEnumerable<JsonConverter>
    {
        public static readonly IEnumerable<JsonConverter> Converters = new List<JsonConverter>()
        {
            new StringEnumConverter(camelCaseText: false),
            new Point2Converter(),
            new Point3Converter(),
            new TimestampedDataJsonConverter(),
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



    class TimestampedDataJsonConverter : JsonConverter<TimestampedData>
    {
        public bool TryParseTimestamps(JObject obj, JsonSerializer serializer, out long trackerTicks, out TimeSpan timestamp)
        {
            JToken tokenTrackerTicks;
            JToken tokenTimestamp;

            bool hasTrackerTicks = obj.TryGetValue(nameof(TimestampedData.TrackerTicks), out tokenTrackerTicks);
            bool hasTimestamp = obj.TryGetValue(nameof(TimestampedData.Timestamp), out tokenTimestamp);

            if (hasTrackerTicks && hasTimestamp)
            {
                trackerTicks = tokenTrackerTicks.ToObject<long>(serializer);
                timestamp = tokenTimestamp.ToObject<TimeSpan>(serializer);
            }
            else if (hasTimestamp)
            {
                timestamp = tokenTimestamp.ToObject<TimeSpan>(serializer);
                trackerTicks = timestamp.Ticks / 10;
            }
            else if (hasTrackerTicks)
            {
                trackerTicks = tokenTrackerTicks.ToObject<long>(serializer);
                timestamp = TimeSpan.Zero;
            }
            else
            {
                trackerTicks = 0L;
                timestamp = TimeSpan.Zero;
            }

            return hasTrackerTicks || hasTimestamp;
        }


        protected override TimestampedData Convert(JToken token, JsonSerializer serializer)
        {
            long trackerTicks;
            TimeSpan timestamp;

            JObject obj = (JObject)token;

            if (TryParseTimestamps(obj, serializer, out trackerTicks, out timestamp))
            {
                return new TimestampedData(trackerTicks, timestamp);
            }

            throw new InvalidOperationException("Missing timestamp in the data.");
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
            var eyePosition3DRelative = obj.GetValue<Point3>(nameof(EyeSample.EyePosition3DRelative), serializer);
            var pupilDiameter = obj.GetValue<double>(nameof(EyeSample.PupilDiameter), serializer);

            return new EyeSample
            (
                gazePoint2D,
                gazePoint3D,
                eyePosition3D,
                eyePosition3DRelative,
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

            // GazeData implements the ITimestampedData interface, we use conversion to the TimestampedData object
            var timestampedData = obj.ToObject<TimestampedData>(serializer);

            var leftEye = obj.GetValue<EyeData>(nameof(GazeData.LeftEye), serializer);
            var rightEye = obj.GetValue<EyeData>(nameof(GazeData.RightEye), serializer);

            return new GazeData(leftEye, rightEye, timestampedData.TrackerTicks, timestampedData.Timestamp);
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
            // SingleEyeGazeData implements the ITimestampedData interface, we can use conversion to the TimestampedData object
            var timestampedData = obj.ToObject<TimestampedData>(serializer);

            // Then we construct the SingleEyeGazeData instance from both objects
            return new SingleEyeGazeData(eyeGazeData, timestampedData.TrackerTicks, timestampedData.Timestamp);
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

            var type = obj.GetValue<EyeMovementType>(nameof(EyeMovement.MovementType), serializer);
            var samples = obj.GetValue<List<EyeVelocity>>(nameof(EyeMovement.Samples), serializer);

            var averageSample = obj.GetValue<EyeSample>(nameof(EyeMovement.AverageSample), serializer);

            var timestampedData = obj.ToObject<TimestampedData>(serializer);

            var endTrackerTicks = obj.GetValue<long>(nameof(EyeMovement.EndTrackerTicks), serializer);
            var endTime = obj.GetValue<TimeSpan>(nameof(EyeMovement.EndTime), serializer);

            return new EyeMovement(type, samples, averageSample, timestampedData.TrackerTicks, timestampedData.Timestamp, endTrackerTicks, endTime);
        }
    }
}
