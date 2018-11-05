using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public class EyeMovement : ITimestampedData
    {
        public static readonly EyeMovement Empty = new EyeMovement(EyeMovementType.Unknown, Enumerable.Empty<EyeVelocity>(), null, 0L, 0L);

        public EyeMovement
        (
            EyeMovementType type, 
            IEnumerable<EyeVelocity> samples, 
            EyeSample averageSample, 
            long trackerTicks, 
            long endTrackerTicks
        )
        {
            Samples = samples?.ToList() ?? new List<EyeVelocity>();

            TrackerTicks = trackerTicks;

            MovementType = type;
            AverageSample = averageSample;

            EndTrackerTicks = endTrackerTicks;
        }


        public List<EyeVelocity> Samples { get; }


        public long TrackerTicks { get; }


        public long EndTrackerTicks { get; }


        public TimeSpan Duration => TimeSpan.FromTicks((EndTrackerTicks - TrackerTicks) * 10);


        public Point2 Position => AverageSample?.GazePoint2D ?? Point2.Zero;


        public EyeSample AverageSample { get; }


        public EyeMovementType MovementType { get; }
    }
}
