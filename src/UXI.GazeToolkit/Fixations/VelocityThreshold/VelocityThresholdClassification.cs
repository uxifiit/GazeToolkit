using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Extensions;

namespace UXI.GazeToolkit.Fixations.VelocityThreshold
{
    public interface IVelocityThresholdClassificationOptions
    {
        double VelocityThreshold { get; }
    }



    public static class VelocityThresholdClassification
    {
        private static EyeMovementType ClassifyVelocity(EyeVelocity velocity, double threshold)
        {
            if (velocity.EyeGazeData.Validity.HasEye() 
                && Double.IsInfinity(velocity.Velocity) == false)
            {
                return velocity.Velocity <= threshold ? EyeMovementType.Fixation : EyeMovementType.Saccade;
            }

            return EyeMovementType.Unknown;
        }


        public static IObservable<EyeMovement> ClassifyByVelocity(this IObservable<EyeVelocity> velocities, double velocityThreshold)
        {
            return velocities.Buffer((first, current) => ClassifyVelocity(first, velocityThreshold) != ClassifyVelocity(current, velocityThreshold))
                             .Where(b => b.Any())
                             .Scan(EyeMovement.Empty, (first, second) =>
                             {
                                 var secondStartTimeTicks = second.First().EyeGazeData.Timestamp.Ticks;
                                 var secondStartTrackerTicks = second.First().EyeGazeData.TrackerTicks;

                                 EyeMovementType movement = ClassifyVelocity(second.First(), velocityThreshold);

                                 if (first != EyeMovement.Empty && first.Samples.Any())
                                 {
                                     var firstEndTimeTicks = first.EndTime.Ticks;
                                     var firstEndTrackerTicks = first.EndTrackerTicks;

                                     var averageTimeTicksDiff = (secondStartTimeTicks - firstEndTimeTicks) / 2;
                                     var averageTrackerTicksDiff = (secondStartTrackerTicks - firstEndTrackerTicks) / 2;

                                     first.EndTime = new TimeSpan(firstEndTimeTicks + averageTimeTicksDiff);
                                     first.EndTrackerTicks = firstEndTrackerTicks + averageTrackerTicksDiff;

                                     secondStartTimeTicks -= averageTimeTicksDiff;
                                     secondStartTrackerTicks -= averageTrackerTicksDiff;
                                 }

                                 return new EyeMovement(second, movement, new TimeSpan(secondStartTimeTicks), secondStartTrackerTicks);
                             });
        }


        public static IObservable<EyeMovement> ClassifyByVelocity(this IObservable<EyeVelocity> velocities, IVelocityThresholdClassificationOptions options)
        {
            return ClassifyByVelocity(velocities, options.VelocityThreshold);
        }
    }
}
