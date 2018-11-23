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
        {
            Samples = samples?.ToList() ?? new List<EyeVelocity>();

            Timestamp = timestamp;

            MovementType = type;
            AverageSample = averageSample;

            EndTimestamp = endTimestamp;
        }


        public List<EyeVelocity> Samples { get; }


        public DateTimeOffset Timestamp { get; }


        public DateTimeOffset EndTimestamp { get; }


        public TimeSpan Duration => EndTimestamp.Subtract(Timestamp);


        public Point2 Position => AverageSample?.GazePoint2D ?? Point2.Zero;


        public EyeSample AverageSample { get; }


        public EyeMovementType MovementType { get; }
    }
}
