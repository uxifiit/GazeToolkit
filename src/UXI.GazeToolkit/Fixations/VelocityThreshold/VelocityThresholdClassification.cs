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
                                 var secondStartTimestamp = second.First().EyeGazeData.Timestamp;

                                 EyeMovementType movement = ClassifyVelocity(second.First(), velocityThreshold);

                                 if (first != EyeMovement.Empty && first.Samples.Any())
                                 {
                                     var firstEndTimestamp = first.EndTime;

                                     var averageTimeTicksDiff = (secondStartTimestamp - firstEndTimestamp) / 2;

                                     first.EndTime = firstEndTimestamp + averageTimeTicksDiff;

                                     secondStartTimestamp -= averageTimeTicksDiff;
                                 }

                                 return new EyeMovement(second, movement, secondStartTimestamp);
                             });
        }


        public static IObservable<EyeMovement> ClassifyByVelocity(this IObservable<EyeVelocity> velocities, IVelocityThresholdClassificationOptions options)
        {
            return ClassifyByVelocity(velocities, options.VelocityThreshold);
        }
    }
}
