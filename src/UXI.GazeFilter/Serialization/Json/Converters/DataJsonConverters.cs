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
            var x = jObject.GetObject<double>(nameof(Point2.X), serializer);
            var y = jObject.GetObject<double>(nameof(Point2.Y), serializer);

            return new Point2(x, y);
        }
    }


    class Point3Converter : JsonConverter<Point3>
    {
        protected override Point3 Convert(JObject jObject, JsonSerializer serializer)
        {
            var x = jObject.GetObject<double>(nameof(Point3.X), serializer);
            var y = jObject.GetObject<double>(nameof(Point3.Y), serializer);
            var z = jObject.GetObject<double>(nameof(Point3.Z), serializer);

            return new Point3(x, y, z);
        }
    }


    class EyeGazeDataConverter : JsonConverter<EyeGazeData>
    {
        protected override EyeGazeData Convert(JObject jObject, JsonSerializer serializer)
        {
            var validity = jObject.GetObject<EyeGazeDataValidity>(nameof(EyeGazeData.Validity), serializer);
            var gazePoint2D = jObject.GetObject<Point2>(nameof(EyeGazeData.GazePoint2D), serializer);
            var gazePoint3D = jObject.GetObject<Point3>(nameof(EyeGazeData.GazePoint3D), serializer);
            var eyePosition3D = jObject.GetObject<Point3>(nameof(EyeGazeData.EyePosition3D), serializer);
            var eyePosition3DRelative = jObject.GetObject<Point3>(nameof(EyeGazeData.EyePosition3DRelative), serializer);
            var pupilDiameter = jObject.GetObject<double>(nameof(EyeGazeData.PupilDiameter), serializer);


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
            var validity = jObject.GetObject<GazeDataValidity>(nameof(GazeData.Validity), serializer);
            var leftEye = jObject.GetObject<EyeGazeData>(nameof(GazeData.LeftEye), serializer);
            var rightEye = jObject.GetObject<EyeGazeData>(nameof(GazeData.RightEye), serializer);
            var timestamp = jObject.GetObject<long>(nameof(GazeData.Timestamp), serializer);

            return new GazeData(validity, leftEye, rightEye, timestamp);
        }
    }


    class SingleEyeGazeDataJsonConverter : JsonConverter<SingleEyeGazeData>
    {
        protected override SingleEyeGazeData Convert(JObject jObject, JsonSerializer serializer)
        {
            // SingleEyeGazeData extends the EyeGazeData class, so we deserialize inherited class first
            var eyeGazeData = jObject.ToObject<EyeGazeData>(serializer);

            // Then we take other members of SingleEyeGazeData
            var timestamp = jObject.GetObject<long>(nameof(SingleEyeGazeData.Timestamp), serializer);

            // And construct the SingleEyeGazeData instance
            return new SingleEyeGazeData(eyeGazeData, timestamp);
        }
    }


    class EyeVelocityJsonConverter : JsonConverter<EyeVelocity>
    {
        protected override EyeVelocity Convert(JObject jObject, JsonSerializer serializer)
        {
            var velocity = jObject.GetObject<double>(nameof(EyeVelocity.Velocity), serializer);
            var eyeGazeData = jObject.GetObject<SingleEyeGazeData>(nameof(EyeVelocity.EyeGazeData), serializer);

            return new EyeVelocity(velocity, eyeGazeData);
        }
    }


    class EyeMovementJsonConverter : JsonConverter<EyeMovement>
    {
        protected override EyeMovement Convert(JObject jObject, JsonSerializer serializer)
        {
            var samples = jObject[nameof(EyeMovement.Samples)].ToObject<List<EyeVelocity>>(serializer);
            var type = jObject.GetObject<EyeMovementType>(nameof(EyeMovement.MovementType), serializer);
            var startTime = jObject.GetObject<long>(nameof(EyeMovement.StartTime), serializer);

            return new EyeMovement(samples, type, startTime);
        }
    }


    class TimestampedJsonConverter : JsonConverter<Timestamped>
    {
        protected override Timestamped Convert(JObject jObject, JsonSerializer serializer)
        {
            long timestamp = jObject.GetObject<long>(nameof(GazeData.Timestamp), serializer);

            return new Timestamped(timestamp);
        }
    }
}
