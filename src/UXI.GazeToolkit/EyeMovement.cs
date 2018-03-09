using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public class EyeMovement
    {
        public static readonly EyeMovement Empty = new EyeMovement(Enumerable.Empty<EyeVelocity>(), EyeMovementType.Saccade, TimeSpan.MinValue, 0L);

        public EyeMovement(IEnumerable<EyeVelocity> samples, EyeMovementType type, TimeSpan startTime, long startTrackerTicks)
        {
            Samples = samples.ToList();

            StartTime = startTime;
            StartTrackerTicks = startTrackerTicks;

            MovementType = type;

            if (samples.Any())
            {
                var lastSample = Samples.Last();

                EndTime = lastSample.EyeGazeData.Timestamp;
                EndTrackerTicks = lastSample.EyeGazeData.TrackerTicks;

                AverageSample = SingleEyeGazeData.AverageRange(samples.Select(s => s.EyeGazeData));
            }
            else
            {
                EndTime = startTime;
                EndTrackerTicks = StartTrackerTicks;
            }
        }


        public List<EyeVelocity> Samples { get; }

        public TimeSpan StartTime { get; }

        public long StartTrackerTicks { get; }

        public TimeSpan EndTime { get; set; }

        public long EndTrackerTicks { get; set; }

        public TimeSpan Duration => EndTime - StartTime;

        public Point2 Position => AverageSample?.GazePoint2D ?? Point2.Default;

        public SingleEyeGazeData AverageSample { get; }

        public EyeMovementType MovementType { get; }
    }
}
