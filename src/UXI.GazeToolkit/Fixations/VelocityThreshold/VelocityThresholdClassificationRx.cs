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
                             .Buffer(2, 1)
                             .Scan<IList<IList<EyeVelocity>>, EyeMovement>(null, (movement, buffers) =>
                             {
                                 var current = buffers.First();
                                 var last = buffers.LastOrDefault();

                                 var currentFirstSample = current.First();

                                 long startTrackerTicks = currentFirstSample.Eye.TrackerTicks;
                                 TimeSpan startTimestamp = currentFirstSample.Eye.Timestamp;

                                 if (movement != null)
                                 {
                                     var startTicksDiff = movement.EndTrackerTicks - movement.Samples.Last().Eye.TrackerTicks;
                                     var startTimestampDiff = movement.EndTime.Ticks - movement.Samples.Last().Eye.Timestamp.Ticks;

                                     startTrackerTicks -= Math.Max(startTicksDiff, 0);
                                     startTimestamp = startTimestamp.Subtract(TimeSpan.FromTicks(Math.Max(startTimestampDiff, 0)));
                                 }

                                 var currentLastSample = current.Last();

                                 long endTrackerTicks = currentLastSample.Eye.TrackerTicks;
                                 TimeSpan endTimestamp = currentLastSample.Eye.Timestamp;

                                 if (last != null && last != current)
                                 {
                                     var endTicksDiff = (last.First().Eye.TrackerTicks - endTrackerTicks) / 2;
                                     var endTimestampDiff = (last.First().Eye.Timestamp.Ticks - endTimestamp.Ticks) / 2;

                                     endTrackerTicks += endTicksDiff;
                                     endTimestamp = endTimestamp.Add(TimeSpan.FromTicks(endTimestampDiff));
                                 }

                                 EyeMovementType movementType = ClassifyMovement(current.First(), velocityThreshold);

                                 EyeSample averageSample = null;
                                 if (movementType == EyeMovementType.Fixation)
                                 {
                                     averageSample = EyeSampleUtils.Average(current.Select(s => s.Eye));
                                 }

                                 return new EyeMovement(movementType, current, averageSample, startTrackerTicks, startTimestamp, endTrackerTicks, endTimestamp);
                             });
        }

     

            //.Buffer(2, 1)
            //.Scan()

            //.Select(tuple =>
            //{
            //    EyeVelocity start = current.First();
            //    EyeVelocity end = current.Last();

            //    long startTrackerTicks = start.Eye.TrackerTicks;
            //    TimeSpan startTime = start.Eye.Timestamp;

            //    long endTrackerTicks = end.Eye.TrackerTicks;
            //    TimeSpan endTime = end.Eye.Timestamp;

            //    EyeMovementType movement = ClassifyMovement(start, velocityThreshold);


            //    if (last != EyeMovement.Empty && last.Samples.Any())
            //    {
            //        long averageTicksDiff = Math.Max(0, (startTrackerTicks - last.EndTrackerTicks) / 2);
            //        TimeSpan averageTimestampDiff = TimeSpan.FromTicks(Math.Max(0, (startTime.Ticks - last.EndTime.Ticks) / 2));

            //        last.EndTrackerTicks += averageTicksDiff;
            //        last.EndTime = last.EndTime.Add(averageTimestampDiff);

            //        startTrackerTicks -= averageTicksDiff;
            //        startTime = startTime.Subtract(averageTimestampDiff);
            //    }

            //    EyeSample averageSample = null;
            //    if (movement == EyeMovementType.Fixation)
            //    {
            //        averageSample = EyeSampleUtils.Average(current.Select(s => s.Eye));
            //    }

            //    return new EyeMovement(movement, current, averageSample, startTrackerTicks, startTime, endTrackerTicks, endTime);
            //});
            //.Scan(EyeMovement.Empty, (last, current) =>
            //{
            //    EyeVelocity start = current.First();
            //    EyeVelocity end = current.Last();

            //    long startTrackerTicks = start.Eye.TrackerTicks;
            //    TimeSpan startTime = start.Eye.Timestamp;

            //    long endTrackerTicks = end.Eye.TrackerTicks;
            //    TimeSpan endTime = end.Eye.Timestamp;

            //    EyeMovementType movement = ClassifyMovement(start, velocityThreshold);


            //    if (last != EyeMovement.Empty && last.Samples.Any())
            //    {
            //        long averageTicksDiff = Math.Max(0, (startTrackerTicks - last.EndTrackerTicks) / 2);
            //        TimeSpan averageTimestampDiff = TimeSpan.FromTicks(Math.Max(0, (startTime.Ticks - last.EndTime.Ticks) / 2));

            //        last.EndTrackerTicks += averageTicksDiff;
            //        last.EndTime = last.EndTime.Add(averageTimestampDiff);

            //        startTrackerTicks -= averageTicksDiff;
            //        startTime = startTime.Subtract(averageTimestampDiff);
            //    }

            //    EyeSample averageSample = null;
            //    if (movement == EyeMovementType.Fixation)
            //    {
            //        averageSample = EyeSampleUtils.Average(current.Select(s => s.Eye));
            //    }

            //    return new EyeMovement(movement, current, averageSample, startTrackerTicks, startTime, endTrackerTicks, endTime);
            //});


        public static IObservable<EyeMovement> ClassifyByVelocity(this IObservable<EyeVelocity> velocities, IVelocityThresholdClassificationOptions options)
        {
            return ClassifyMovements(velocities, options.VelocityThreshold);
        }
    }
}
