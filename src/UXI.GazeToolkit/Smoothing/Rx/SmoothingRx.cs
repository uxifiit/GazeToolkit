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
        NoiseReductionStrategy Strategy { get; } 
    }



    public interface IExponentialSmoothingOptions : INoiseReductionOptions
    {
        double Alpha { get; }
    }



    public interface IMovingAverageSmoothingOptions : INoiseReductionOptions
    {
        int WindowSize { get; }
    }



    public static class SmoothingRx
    {
        private static ISingleEyeGazeDataSmoothingFilter ResolveSmoothingFilter(INoiseReductionOptions options)
        {
            switch (options.Strategy)
            {
                case NoiseReductionStrategy.Exponential:
                    var exponentialOptions = options as IExponentialSmoothingOptions;
                    return new ExponentialSmoothingFilter(exponentialOptions.Alpha);
                case NoiseReductionStrategy.MovingAverage:
                    var movingAverageOptions = options as IMovingAverageSmoothingOptions;
                    return new MovingAverageSmoothingFilter(movingAverageOptions.WindowSize);
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
