using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Smoothing
{
    public enum NoiseReductionStrategy
    {
        MovingAverage,
        Exponential,
        Median
    }



    public interface INoiseReductionOptions
    {
        NoiseReductionStrategy NoiseReductionStrategy { get; } 
    }



    public interface IExponentialSmoothingOptions : INoiseReductionOptions
    {
        double Alpha { get; }
    }



    public interface IMovingAverageSmoothingOptions : INoiseReductionOptions
    {
        int WindowSize { get; }
    }



    public interface IMedianSmoothingOptions : INoiseReductionOptions
    {
        int WindowSize { get; }
    }



    public static class SmoothingRx
    {
        private static ISingleEyeGazeDataSmoothingFilter ResolveSmoothingFilter(INoiseReductionOptions options)
        {
            switch (options.NoiseReductionStrategy)
            {
                case NoiseReductionStrategy.Exponential:
                    var exponentialOptions = options as IExponentialSmoothingOptions;
                    return exponentialOptions != null
                         ? new ExponentialSmoothingFilter(exponentialOptions.Alpha)
                         : new ExponentialSmoothingFilter();
                case NoiseReductionStrategy.MovingAverage:
                    var movingAverageOptions = options as IMovingAverageSmoothingOptions;
                    return movingAverageOptions != null
                         ? new MovingAverageSmoothingFilter(movingAverageOptions.WindowSize)
                         : new MovingAverageSmoothingFilter();
                case NoiseReductionStrategy.Median:
                default:
                    throw new NotImplementedException();
            }   
        }


        public static IObservable<SingleEyeGazeData> ReduceNoise(this IObservable<SingleEyeGazeData> gazeData, ISingleEyeGazeDataSmoothingFilter smoothing)
        {
            return smoothing.Smooth(gazeData);
        }


        public static IObservable<SingleEyeGazeData> ReduceNoise(this IObservable<SingleEyeGazeData> gazeData, INoiseReductionOptions options)
        {
            return ReduceNoise(gazeData, ResolveSmoothingFilter(options));
        }
    }
}
