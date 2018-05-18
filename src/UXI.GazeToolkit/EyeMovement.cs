using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public class EyeMovement
    {
        public static readonly EyeMovement Empty = new EyeMovement(Enumerable.Empty<EyeVelocity>(), EyeMovementType.Saccade, 0L);

        public EyeMovement(IEnumerable<EyeVelocity> samples, EyeMovementType type, long startTime)
        {
            Samples = samples.ToList();

            StartTime = startTime;

            MovementType = type;

            if (samples.Any())
            {
                var lastSample = Samples.Last();

                EndTime = lastSample.EyeGazeData.Timestamp;

                AverageSample = SingleEyeGazeData.AverageRange(samples.Select(s => s.EyeGazeData));
            }
            else
            {
                EndTime = startTime;
            }
        }


        public List<EyeVelocity> Samples { get; }

        public long StartTime { get; }

        public long EndTime { get; set; }

        public TimeSpan Duration => TimeSpan.FromTicks((EndTime - StartTime) * 10);

        public Point2 Position => AverageSample?.GazePoint2D ?? Point2.Default;

        public SingleEyeGazeData AverageSample { get; }

        public EyeMovementType MovementType { get; }
    }
}
