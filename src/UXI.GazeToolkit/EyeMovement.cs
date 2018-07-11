using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public class EyeMovement
    {
        public static readonly EyeMovement Empty = new EyeMovement(Enumerable.Empty<EyeVelocity>(), EyeMovementType.Unknown, null, 0L, TimeSpan.Zero);

        public EyeMovement(IEnumerable<EyeVelocity> samples, EyeMovementType type, EyeSample averageSample, long startTrackerTicks, TimeSpan startTime)
        {
            Samples = samples?.ToList() ?? new List<EyeVelocity>();

            StartTrackerTicks = startTrackerTicks;
            StartTime = startTime;

            MovementType = type;
            AverageSample = averageSample;

            if (Samples.Any())
            {
                var lastSample = Samples.Last();

                EndTrackerTicks = lastSample.Eye.TrackerTicks;
                EndTime = lastSample.Eye.Timestamp;
            }
            else
            {
                EndTrackerTicks = startTrackerTicks;
                EndTime = startTime;
            }
        }


        public List<EyeVelocity> Samples { get; }

        public TimeSpan StartTime { get; }

        public long StartTrackerTicks { get; }

        public TimeSpan EndTime { get; set; }

        public long EndTrackerTicks { get; set; }

        public TimeSpan Duration => TimeSpan.FromTicks((EndTrackerTicks - StartTrackerTicks) * 10);

        public Point2 Position => AverageSample?.GazePoint2D ?? Point2.Zero;

        public EyeSample AverageSample { get; }

        public EyeMovementType MovementType { get; }
    }
}
