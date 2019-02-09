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

        // TODO AveragingStrategy
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
                             .Buffer(2, 1)
                             .Scan<IList<IList<EyeVelocity>>, EyeMovement>(null, (movement, buffers) =>
                             {
                                 var current = buffers.First();
                                 var last = buffers.LastOrDefault();

                                 var currentFirstSample = current.First();
                                 DateTimeOffset startTimestamp = currentFirstSample.Eye.Timestamp;

                                 if (movement != null)
                                 {
                                     var startTicksDiff = movement.EndTimestamp - movement.Samples.Last().Eye.Timestamp;

                                     startTimestamp = startTimestamp.Subtract(startTicksDiff.Duration());
                                 }

                                 var currentLastSample = current.Last();
                                 DateTimeOffset endTimestamp = currentLastSample.Eye.Timestamp;

                                 if (last != null && last != current)
                                 {
                                     var endTicksDiff = (last.First().Eye.Timestamp - endTimestamp).Duration().Ticks / 2;

                                     endTimestamp = endTimestamp.Add(TimeSpan.FromTicks(endTicksDiff));
                                 }

                                 EyeMovementType movementType = ClassifyMovement(current.First(), velocityThreshold);

                                 EyeSample averageSample = null;
                                 if (movementType == EyeMovementType.Fixation)
                                 {
                                     averageSample = EyeSampleUtils.Average(current.Select(s => s.Eye));
                                 }

                                 return new EyeMovement(movementType, current, averageSample, startTimestamp, endTimestamp);
                             });
        }


        public static IObservable<EyeMovement> ClassifyByVelocity(this IObservable<EyeVelocity> velocities, IVelocityThresholdClassificationOptions options)
        {
            return ClassifyMovements(velocities, options.VelocityThreshold);
        }
    }
}
