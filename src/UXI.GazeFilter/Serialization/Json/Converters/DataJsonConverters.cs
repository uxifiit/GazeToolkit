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

namespace UXI.Data.Serialization.Json.Converters
{
    public class DataJsonConverters : IEnumerable<JsonConverter>
    {
        public static readonly IEnumerable<JsonConverter> Converters = new List<JsonConverter>()
        {
            new StringEnumConverter(camelCaseText: false),
            new Point2Converter(),
            new Point3Converter(),
            new EyeGazeDataConverter(),
            new GazeDataConverter(),
            new EyeVelocityJsonConverter(),
            new SingleEyeGazeDataJsonConverter(),
            new TimestampedJsonConverter(),
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


    class Point2Converter : JsonConverter<Point2>
    {
        protected override Point2 Convert(JObject jObject, JsonSerializer serializer)
        {
            var x = jObject[nameof(Point2.X)].ToObject<double>(serializer);
            var y = jObject[nameof(Point2.Y)].ToObject<double>(serializer);

            return new Point2(x, y);
        }
    }


    class Point3Converter : JsonConverter<Point3>
    {
        protected override Point3 Convert(JObject jObject, JsonSerializer serializer)
        {
            var x = jObject[nameof(Point3.X)].ToObject<double>(serializer);
            var y = jObject[nameof(Point3.Y)].ToObject<double>(serializer);
            var z = jObject[nameof(Point3.Z)].ToObject<double>(serializer);

            return new Point3(x, y, z);
        }
    }


    class EyeGazeDataConverter : JsonConverter<EyeGazeData>
    {
        protected override EyeGazeData Convert(JObject jObject, JsonSerializer serializer)
        {
            var validity = jObject[nameof(EyeGazeData.Validity)].ToObject<EyeGazeDataValidity>(serializer);
            var gazePoint2D = jObject[nameof(EyeGazeData.GazePoint2D)].ToObject<Point2>(serializer);
            var gazePoint3D = jObject[nameof(EyeGazeData.GazePoint3D)].ToObject<Point3>(serializer);
            var eyePosition3D = jObject[nameof(EyeGazeData.EyePosition3D)].ToObject<Point3>(serializer);
            var eyePosition3DRelative = jObject[nameof(EyeGazeData.EyePosition3DRelative)].ToObject<Point3>(serializer);
            var pupilDiameter = jObject[nameof(EyeGazeData.PupilDiameter)].ToObject<double>(serializer);


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
        protected override GazeData Convert(JObject jObject, JsonSerializer serializer)
        {
            var validity = jObject[nameof(GazeData.Validity)].ToObject<GazeDataValidity>(serializer);
            var trackerTicks = jObject[nameof(GazeData.TrackerTicks)].ToObject<long>(serializer);
            var leftEye = jObject[nameof(GazeData.LeftEye)].ToObject<EyeGazeData>(serializer);
            var rightEye = jObject[nameof(GazeData.RightEye)].ToObject<EyeGazeData>(serializer);
            var timestamp = jObject[nameof(GazeData.Timestamp)].ToObject<TimeSpan>(serializer);

            return new GazeData(validity, leftEye, rightEye, trackerTicks, timestamp);
        }
    }


    class SingleEyeGazeDataJsonConverter : JsonConverter<SingleEyeGazeData>
    {
        protected override SingleEyeGazeData Convert(JObject jObject, JsonSerializer serializer)
        {
            // SingleEyeGazeData extends the EyeGazeData class, so we deserialize inherited class first
            var eyeGazeData = jObject.ToObject<EyeGazeData>(serializer);

            // Then we take other members of SingleEyeGazeData
            var trackerTicks = jObject[nameof(SingleEyeGazeData.TrackerTicks)].ToObject<long>(serializer);
            var timestamp = jObject[nameof(SingleEyeGazeData.Timestamp)].ToObject<TimeSpan>(serializer);

            // And construct the SingleEyeGazeData instance
            return new SingleEyeGazeData(eyeGazeData, trackerTicks, timestamp);
        }
    }


    class EyeVelocityJsonConverter : JsonConverter<EyeVelocity>
    {
        protected override EyeVelocity Convert(JObject jObject, JsonSerializer serializer)
        {
            var velocity = jObject[nameof(EyeVelocity.Velocity)].ToObject<double>(serializer);
            var eyeGazeData = jObject[nameof(EyeVelocity.EyeGazeData)].ToObject<SingleEyeGazeData>(serializer);

            return new EyeVelocity(velocity, eyeGazeData);
        }
    }


    class EyeMovementJsonConverter : JsonConverter<EyeMovement>
    {
        protected override EyeMovement Convert(JObject jObject, JsonSerializer serializer)
        {
            var samples = jObject[nameof(EyeMovement.Samples)].ToObject<List<EyeVelocity>>(serializer);
            var type = jObject[nameof(EyeMovement.MovementType)].ToObject<EyeMovementType>(serializer);
            var startTime = jObject[nameof(EyeMovement.StartTime)].ToObject<TimeSpan>(serializer);
            var startTrackerTicks = jObject[nameof(EyeMovement.StartTrackerTicks)].ToObject<long>(serializer);

            return new EyeMovement(samples, type, startTime, startTrackerTicks);
        }
    }


    class TimestampedJsonConverter : JsonConverter<Timestamped>
    {
        protected override Timestamped Convert(JObject jObject, JsonSerializer serializer)
        {
            var timestamp = jObject[nameof(GazeData.Timestamp)].ToObject<TimeSpan>(serializer);

            return new Timestamped(timestamp);
        }
    }
}
