using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Extensions;
using UXI.GazeToolkit.Utils;

namespace UXI.GazeToolkit.Fixations.VelocityThreshold
{
    public interface IVelocityThresholdClassificationOptions
    {
        double VelocityThreshold { get; }

        //AveragingStrategy
    }



    public static class VelocityThresholdClassificationRx
    {
        private static EyeMovementType ClassifyMovement(EyeVelocity velocity, double threshold)
        {
            if (velocity.Eye.Validity.HasEye() 
                && Double.IsInfinity(velocity.Velocity) == false)
            {
                return velocity.Velocity > threshold ? EyeMovementType.Saccade : EyeMovementType.Fixation;
            }

            return EyeMovementType.Unknown;
        }


        public static IObservable<EyeMovement> ClassifyMovements(this IObservable<EyeVelocity> velocities, double velocityThreshold)
        {
            return velocities.Buffer((first, current) => ClassifyMovement(first, velocityThreshold) != ClassifyMovement(current, velocityThreshold))
                             .Where(b => b.Any())
                             .Scan(EyeMovement.Empty, (first, second) =>
                             {
                                 EyeVelocity secondStart = second.First();

                                 var secondStartTrackerTicks = secondStart.Eye.TrackerTicks;
                                 var secondStartTimestamp = secondStart.Eye.Timestamp;

                                 EyeMovementType movement = ClassifyMovement(secondStart, velocityThreshold);

                                 EyeSample averageSample = null;

                                 if (first != EyeMovement.Empty && first.Samples.Any())
                                 {
                                     var firstEndTrackerTicks = first.EndTrackerTicks;
                                     var firstEndTimestamp = first.EndTime;

                                     var averageTicksDiff = Math.Max(0, (secondStartTrackerTicks - firstEndTrackerTicks) / 2);
                                     var averageTimestampDiff = TimeSpan.FromTicks(Math.Max(0, (secondStartTimestamp.Ticks - firstEndTimestamp.Ticks) / 2));

                                     first.EndTrackerTicks += averageTicksDiff;
                                     first.EndTime = first.EndTime.Add(averageTimestampDiff);

                                     secondStartTrackerTicks -= averageTicksDiff;
                                     secondStartTimestamp = secondStartTimestamp.Subtract(averageTimestampDiff);

                                     averageSample = EyeSampleUtils.Average(first.Samples.Select(s => s.Eye));
                                 }

                                 return new EyeMovement(second, movement, averageSample, secondStartTrackerTicks, secondStartTimestamp);
                             });
        }


        public static IObservable<EyeMovement> ClassifyByVelocity(this IObservable<EyeVelocity> velocities, IVelocityThresholdClassificationOptions options)
        {
            return ClassifyMovements(velocities, options.VelocityThreshold);
        }
    }
}
