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
            new EyeGazeDataConverter(),
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



    class EyeGazeDataConverter : JsonConverter<EyeGazeData>
    {
        protected override EyeGazeData Convert(JObject obj, JsonSerializer serializer)
        {
            var validity = obj.GetObject<EyeGazeDataValidity>(nameof(EyeGazeData.Validity), serializer);
            var gazePoint2D = obj.GetObject<Point2>(nameof(EyeGazeData.GazePoint2D), serializer);
            var gazePoint3D = obj.GetObject<Point3>(nameof(EyeGazeData.GazePoint3D), serializer);
            var eyePosition3D = obj.GetObject<Point3>(nameof(EyeGazeData.EyePosition3D), serializer);
            var eyePosition3DRelative = obj.GetObject<Point3>(nameof(EyeGazeData.EyePosition3DRelative), serializer);
            var pupilDiameter = obj.GetObject<double>(nameof(EyeGazeData.PupilDiameter), serializer);


            return new EyeGazeData
            (
                validity,
                gazePoint2D,
                gazePoint3D,
                eyePosition3D,
                eyePosition3DRelative,
                pupilDiameter
            );
        }
    }



    class GazeDataConverter : JsonConverter<GazeData>
    {
        protected override GazeData Convert(JObject obj, JsonSerializer serializer)
        {
            // GazeData implements the ITimestampedData interface, we use conversion to the TimestampedData object
            var timestampedData = obj.ToObject<TimestampedData>(serializer);

            var validity = obj.GetObject<GazeDataValidity>(nameof(GazeData.Validity), serializer);
            var leftEye = obj.GetObject<EyeGazeData>(nameof(GazeData.LeftEye), serializer);
            var rightEye = obj.GetObject<EyeGazeData>(nameof(GazeData.RightEye), serializer);

            return new GazeData(validity, leftEye, rightEye, timestampedData.TrackerTicks, timestampedData.Timestamp);
        }
    }



    class SingleEyeGazeDataJsonConverter : JsonConverter<SingleEyeGazeData>
    {
        protected override SingleEyeGazeData Convert(JObject obj, JsonSerializer serializer)
        {
            // SingleEyeGazeData extends the EyeGazeData class, so we deserialize inherited class first
            var eyeGazeData = obj.ToObject<EyeGazeData>(serializer);

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
            var eyeGazeData = obj.GetObject<SingleEyeGazeData>(nameof(EyeVelocity.EyeGazeData), serializer);

            return new EyeVelocity(velocity, eyeGazeData);
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
