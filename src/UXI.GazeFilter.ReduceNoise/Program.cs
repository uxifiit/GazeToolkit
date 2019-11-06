using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.Filters;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Fixations.VelocityThreshold;
using UXI.GazeToolkit.Smoothing;

namespace UXI.GazeFilter.ReduceNoise
{
    [Verb("movavg")]
    public class MovingAverageSmoothingOptions : BaseOptions, IMovingAverageSmoothingOptions
    {
        public NoiseReductionStrategy NoiseReductionStrategy => NoiseReductionStrategy.MovingAverage;

        [Option('w', "window-size", Default = MovingAverageSmoothingFilter.DEFAULT_WINDOW_SIZE, HelpText = "Window size for Moving Average smoothing filter", Required = false)]
        public int WindowSize { get; set; }
    }



    [Verb("exponential")]
    public class ExponentialSmoothingOptions : BaseOptions, IExponentialSmoothingOptions
    {
        public NoiseReductionStrategy NoiseReductionStrategy => NoiseReductionStrategy.Exponential;

        [Option('a', "alpha", Default = ExponentialSmoothingFilter.DEFAULT_ALPHA, HelpText = "Alpha parameter for Exponential fmoothing filter", Required = false)]
        public double Alpha { get; set; }
    }



    static class Program
    {
        static int Main(string[] args)
        {
            return new MultiFilterHost<GazeFilterContext>
            (
                new RelayFilter<SingleEyeGazeData, SingleEyeGazeData, MovingAverageSmoothingOptions>("Reduce noise - Moving average smoothing", (s, o, _) => s.ReduceNoise(o)),
                new RelayFilter<SingleEyeGazeData, SingleEyeGazeData, ExponentialSmoothingOptions>("Reduce noise - Exponential smoothing", (s, o, _) => s.ReduceNoise(o))
            ).Execute(args);
        }
    }
}
