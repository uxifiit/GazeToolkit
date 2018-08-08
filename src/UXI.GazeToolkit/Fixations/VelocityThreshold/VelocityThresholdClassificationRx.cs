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
                             .Scan(EyeMovement.Empty, (last, current) =>
                             {
                                 EyeVelocity start = current.First();
                                 EyeVelocity end = current.Last();

                                 long startTrackerTicks = start.Eye.TrackerTicks;
                                 TimeSpan startTime = start.Eye.Timestamp;

                                 long endTrackerTicks = end.Eye.TrackerTicks;
                                 TimeSpan endTime = end.Eye.Timestamp;

                                 EyeMovementType movement = ClassifyMovement(start, velocityThreshold);

                                 EyeSample averageSample = null;

                                 if (last != EyeMovement.Empty && last.Samples.Any())
                                 {
                                     long averageTicksDiff = Math.Max(0, (startTrackerTicks - last.EndTrackerTicks) / 2);
                                     TimeSpan averageTimestampDiff = TimeSpan.FromTicks(Math.Max(0, (startTime.Ticks - last.EndTime.Ticks) / 2));

                                     last.EndTrackerTicks += averageTicksDiff;
                                     last.EndTime = last.EndTime.Add(averageTimestampDiff);

                                     startTrackerTicks -= averageTicksDiff;
                                     startTime = startTime.Subtract(averageTimestampDiff);

                                     averageSample = EyeSampleUtils.Average(last.Samples.Select(s => s.Eye));
                                 }

                                 return new EyeMovement(movement, current, averageSample, startTrackerTicks, startTime, endTrackerTicks, endTime);
                             });
        }


        public static IObservable<EyeMovement> ClassifyByVelocity(this IObservable<EyeVelocity> velocities, IVelocityThresholdClassificationOptions options)
        {
            return ClassifyMovements(velocities, options.VelocityThreshold);
        }
    }
}
