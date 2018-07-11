using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Extensions;

namespace UXI.GazeToolkit.Fixations.VelocityThreshold
{
    public interface IVelocityCalculationOptions
    {
        TimeSpan TimeWindowSide { get; }
        int? DataFrequency { get; } 
    }



    public static class VelocityCalculationRx
    {
        public const double DefaultTimeWindowSideMilliseconds = 20d;
        public static readonly TimeSpan DefaultTimeWindowSide = TimeSpan.FromMilliseconds(DefaultTimeWindowSideMilliseconds);


        private static EyeVelocity CalculateVelocity(SingleEyeGazeData sample, SingleEyeGazeData fromSample, SingleEyeGazeData toSample)
        {
            double velocity = 0;

            if (fromSample.Validity.HasEye() && toSample.Validity.HasEye() && sample.Validity.HasEye())
            {
                double visualAngleDegrees = sample.GetVisualAngle(fromSample, toSample);

                if (Double.IsNaN(visualAngleDegrees))
                {
                    velocity = 0;
                }
                else
                {
                    double durationSeconds = (Math.Abs(toSample.TrackerTicks - fromSample.TrackerTicks) / (1000 * 1000d));
                    velocity = visualAngleDegrees / durationSeconds;
                    if (Double.IsNaN(velocity))
                    {
                        velocity = 0;
                    }
                }
            }

            return new EyeVelocity(velocity, sample);
        }


        public static IObservable<EyeVelocity> CalculateVelocities(this IObservable<SingleEyeGazeData> gazeData, TimeSpan timeWindowSide, int frequency)
        {
            // how many samples are in 1 second
            double averagedTime = 1000d / frequency;

            // get number of samples in one side of the window
            int sampleWindowSide = Math.Max(1, (int)Math.Round(timeWindowSide.TotalMilliseconds / averagedTime));

            // create window with two sides
            int windowSize = sampleWindowSide * 2 + 1;

            return gazeData.Buffer(windowSize, 1)
                           .Where(buffer => buffer.Count == windowSize)
                           .Select(buffer =>
                           {
                               var first = buffer.First();
                               var last = buffer.Last();
                               var sample = buffer[buffer.Count / 2];

                               return CalculateVelocity(sample, first, last);
                           });
        }


        public static IObservable<EyeVelocity> CalculateVelocities(this IObservable<SingleEyeGazeData> gazeData, IVelocityCalculationOptions options)
        {
            if (options.DataFrequency.HasValue)
            {
                return CalculateVelocities(gazeData, options.TimeWindowSide, options.DataFrequency.Value);
            }


            // TODO calc frequency as IObservable

            throw new NotImplementedException("Online frequency calculation has not been implemented for the velocity calculation.");
        }
    }
}