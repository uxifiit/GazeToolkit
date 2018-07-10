using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Fixations
{
    public interface IFixationsDiscardingOptions
    {
        TimeSpan MinimumFixationDuration { get; }
    }



    public static class FixationsDiscardingRx
    {
        public static IObservable<EyeMovement> DiscardShortFixations(this IObservable<EyeMovement> movements, TimeSpan minimumFixationDuration)
        {
            return movements.Select(m =>
            {
                // if fixation duration is shorter than minimumFixationDuration
                if (m.MovementType == EyeMovementType.Fixation && m.Duration <= minimumFixationDuration)    // TODO <
                {
                    // we reclassify it as Unknown movement
                    return new EyeMovement(m.Samples, EyeMovementType.Unknown, m.AverageSample, m.StartTrackerTicks, m.StartTime)
                    {
                        EndTrackerTicks = m.EndTrackerTicks,
                        EndTime = m.EndTime
                    };
                }
                return m;
            });
        }


        public static IObservable<EyeMovement> DiscardShortFixations(this IObservable<EyeMovement> movements, IFixationsDiscardingOptions options)
        {
            return DiscardShortFixations(movements, options.MinimumFixationDuration);
        }
    }
}
