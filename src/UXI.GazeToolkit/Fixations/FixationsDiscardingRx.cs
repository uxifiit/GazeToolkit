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
            return movements.Select(movement =>
            {
                // if the fixation duration is shorter than minimumFixationDuration
                if (movement.MovementType == EyeMovementType.Fixation && movement.Duration < minimumFixationDuration)
                {
                    // we reclassify it as an unknown movement
                    return new EyeMovement
                    (
                        EyeMovementType.Unknown, 
                        movement.Samples, 
                        null, 
                        movement.Timestamp, 
                        movement.EndTimestamp 
                    );
                }
                return movement;
            });
        }


        public static IObservable<EyeMovement> DiscardShortFixations(this IObservable<EyeMovement> movements, IFixationsDiscardingOptions options)
        {
            return DiscardShortFixations(movements, options.MinimumFixationDuration);
        }
    }
}
