using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public class EyeMovement : ITimestampedData
    {
        public static readonly EyeMovement Empty = new EyeMovement(EyeMovementType.Unknown, Enumerable.Empty<EyeVelocity>(), null, 0L, TimeSpan.Zero, 0L, TimeSpan.Zero);

        public EyeMovement(EyeMovementType type, IEnumerable<EyeVelocity> samples, EyeSample averageSample, long trackerTicks, TimeSpan timestamp, long endTrackerTicks, TimeSpan endTime)
        {
            Samples = samples?.ToList() ?? new List<EyeVelocity>();

            TrackerTicks = trackerTicks;
            Timestamp = timestamp;

            MovementType = type;
            AverageSample = averageSample;

            EndTrackerTicks = endTrackerTicks;
            EndTime = endTime;
        }



        public List<EyeVelocity> Samples { get; }

        public TimeSpan Timestamp { get; }

        public long TrackerTicks { get; }

        public TimeSpan EndTime { get; set; }

        public long EndTrackerTicks { get; set; }

        public TimeSpan Duration => TimeSpan.FromTicks((EndTrackerTicks - TrackerTicks) * 10);

        public Point2 Position => AverageSample?.GazePoint2D ?? Point2.Zero;

        public EyeSample AverageSample { get; }

        public EyeMovementType MovementType { get; }
    }
}
