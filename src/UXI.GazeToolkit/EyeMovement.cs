using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public class EyeMovement : ITimestampedData
    {
        public static readonly EyeMovement Empty = new EyeMovement(EyeMovementType.Unknown, Enumerable.Empty<EyeVelocity>(), null, DateTimeOffset.MinValue, DateTimeOffset.MinValue);

        public EyeMovement
        (
            EyeMovementType type, 
            IEnumerable<EyeVelocity> samples, 
            EyeSample averageSample, 
            DateTimeOffset timestamp,
            DateTimeOffset endTimestamp
        )
            : this
        (
            type,
            averageSample,
            timestamp,
            endTimestamp
        )
        {
            Samples = samples?.ToList() ?? Samples;
        }


        public EyeMovement
        (
            EyeMovementType type,
            EyeSample averageSample,
            DateTimeOffset timestamp,
            DateTimeOffset endTimestamp
        )
            : this
        (
            type,
            timestamp,
            endTimestamp
        )
        {
            AverageSample = averageSample;
        }


        public EyeMovement
        (
            EyeMovementType type,
            Point2 gazePoint,
            DateTimeOffset timestamp,
            DateTimeOffset endTimestamp
        )
            : this
        (
            type,
            timestamp,
            endTimestamp
        )
        {
            position = gazePoint;
        }


        private EyeMovement
        (
            EyeMovementType type,
            DateTimeOffset timestamp,
            DateTimeOffset endTimestamp
        )
        {
            MovementType = type;
            Timestamp = timestamp;
            EndTimestamp = endTimestamp;
        }


        public List<EyeVelocity> Samples { get; } = new List<EyeVelocity>();


        public DateTimeOffset Timestamp { get; }


        public DateTimeOffset EndTimestamp { get; }


        public TimeSpan Duration => EndTimestamp.Subtract(Timestamp);


        private Point2? position;
        public Point2? Position => position ?? AverageSample?.GazePoint2D;


        public EyeSample AverageSample { get; }


        public EyeMovementType MovementType { get; }
    }
}
