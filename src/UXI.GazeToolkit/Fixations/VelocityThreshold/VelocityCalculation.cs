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
                double visualAngle = sample.GetVisualAngle(fromSample, toSample);

                if (Double.IsNaN(visualAngle))
                {
                    velocity = 0;  // should be maxVelocity - 1
                }
                else
                {
                    velocity = visualAngle / ((toSample.Timestamp - fromSample.Timestamp) / 1000000d);
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
            double averagedTime = 1000d / frequency;

            int sampleWindowSide = Math.Max(1, (int)Math.Round(timeWindowSide.TotalMilliseconds / averagedTime));

            int bufferSize = sampleWindowSide * 2 + 1;

            return gazeData.Buffer(bufferSize, 1)
                           .Where(buffer => buffer.Count == bufferSize)
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
            throw new NotImplementedException();
        }
    }
}
