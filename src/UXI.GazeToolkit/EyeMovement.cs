using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public class EyeMovement
    {
        public static readonly EyeMovement Empty = new EyeMovement(Enumerable.Empty<EyeVelocity>(), EyeMovementType.Saccade, null, 0L, TimeSpan.Zero);

        public EyeMovement(IEnumerable<EyeVelocity> samples, EyeMovementType type, EyeGazeData averageSample, long startTrackerTicks, TimeSpan startTime)
        {
            Samples = samples.ToList();

            StartTrackerTicks = startTrackerTicks;
            StartTime = startTime;

            MovementType = type;
            AverageSample = averageSample;

            if (samples.Any())
            {
                var lastSample = Samples.Last();

                EndTrackerTicks = lastSample.EyeGazeData.TrackerTicks;
                EndTime = lastSample.EyeGazeData.Timestamp;
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

        public EyeGazeData AverageSample { get; }

        public EyeMovementType MovementType { get; }
    }
}
