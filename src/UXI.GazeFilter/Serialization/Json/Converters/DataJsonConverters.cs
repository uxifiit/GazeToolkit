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
            //new EyeMovementJsonConverter()
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



    class Point2Converter : JsonConverter<Point2>
    {
        protected override Point2 Convert(JObject obj, JsonSerializer serializer)
        {
            var x = obj.GetObject<double>(nameof(Point2.X), serializer);
            var y = obj.GetObject<double>(nameof(Point2.Y), serializer);

            return new Point2(x, y);
        }
    }



    class Point3Converter : JsonConverter<Point3>
    {
        protected override Point3 Convert(JObject obj, JsonSerializer serializer)
        {
            var x = obj.GetObject<double>(nameof(Point3.X), serializer);
            var y = obj.GetObject<double>(nameof(Point3.Y), serializer);
            var z = obj.GetObject<double>(nameof(Point3.Z), serializer);

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


        protected override TimestampedData Convert(JObject obj, JsonSerializer serializer)
        {
            long trackerTicks;
            TimeSpan timestamp;

            if (TryParseTimestamps(obj, serializer, out trackerTicks, out timestamp))
            {
                return new TimestampedData(trackerTicks, timestamp);
            }

            throw new InvalidOperationException("Missing timestamp in the data.");
        }
    }


    class EyeSampleConverter : JsonConverter<EyeSample>
    {
        protected override EyeSample Convert(JObject obj, JsonSerializer serializer)
        {
            var gazePoint2D = obj.GetObject<Point2>(nameof(EyeSample.GazePoint2D), serializer);
            var gazePoint3D = obj.GetObject<Point3>(nameof(EyeSample.GazePoint3D), serializer);
            var eyePosition3D = obj.GetObject<Point3>(nameof(EyeSample.EyePosition3D), serializer);
            var eyePosition3DRelative = obj.GetObject<Point3>(nameof(EyeSample.EyePosition3DRelative), serializer);
            var pupilDiameter = obj.GetObject<double>(nameof(EyeSample.PupilDiameter), serializer);

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
        protected override EyeData Convert(JObject obj, JsonSerializer serializer)
        {
            var sample = obj.ToObject<EyeSample>(serializer);

            var validity = obj.GetObject<EyeValidity>(nameof(EyeData.Validity), serializer);

            return new EyeData
            (
                validity,
                sample
            );
        }
    }


    class GazeDataConverter : JsonConverter<GazeData>
    {
        protected override GazeData Convert(JObject obj, JsonSerializer serializer)
        {
            // GazeData implements the ITimestampedData interface, we use conversion to the TimestampedData object
            var timestampedData = obj.ToObject<TimestampedData>(serializer);

            var leftEye = obj.GetObject<EyeData>(nameof(GazeData.LeftEye), serializer);
            var rightEye = obj.GetObject<EyeData>(nameof(GazeData.RightEye), serializer);

            return new GazeData(leftEye, rightEye, timestampedData.TrackerTicks, timestampedData.Timestamp);
        }
    }



    class SingleEyeGazeDataJsonConverter : JsonConverter<SingleEyeGazeData>
    {
        protected override SingleEyeGazeData Convert(JObject obj, JsonSerializer serializer)
        {
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
        protected override EyeVelocity Convert(JObject obj, JsonSerializer serializer)
        {
            var velocity = obj.GetObject<double>(nameof(EyeVelocity.Velocity), serializer);
            var eye = obj.GetObject<SingleEyeGazeData>(nameof(EyeVelocity.Eye), serializer);

            return new EyeVelocity(velocity, eye);
        }
    }



    //class EyeMovementJsonConverter : JsonConverter<EyeMovement>
    //{
    //    protected override EyeMovement Convert(JObject obj, JsonSerializer serializer)
    //    {
    //        var samples = obj[nameof(EyeMovement.Samples)].ToObject<List<EyeVelocity>>(serializer);
    //        var type = obj.GetObject<EyeMovementType>(nameof(EyeMovement.MovementType), serializer);

    //        var startTime = obj.GetObject<long>(nameof(EyeMovement.StartTrackerTicks), serializer);

    //        return new EyeMovement(samples, type, startTime);
    //    }
    //}
}
