using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.Filters;
using UXI.GazeFilter;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Fixations.VelocityThreshold;

namespace UXI.GazeFilter.VelocityCalculation
{
    public class VelocityCalculationOptions : BaseOptions, IVelocityCalculationOptions
    {
        [Option('f', "frequency", Default = null, SetName = "frequency", HelpText = "Frequency of the eye tracking data.")]
        public int? DataFrequency { get; set; }


        [Option('w', "window-side", Default = VelocityCalculationRx.DefaultTimeWindowHalfDurationMilliseconds, SetName = "frequency", HelpText = "Time window length in milliseconds to use for measuring frequency of the eye tracking data")]
        public double TimeWindowHalfDurationMilliseconds
        {
            get
            {
                return TimeWindowHalfDuration.TotalMilliseconds;
            }
            set
            {
                TimeWindowHalfDuration = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan TimeWindowHalfDuration { get; private set; }
    }



    static class Program
    {
        static int Main(string[] args)
        {
            return new SingleFilterHost<GazeFilterContext, VelocityCalculationOptions>
            (
                new RelayFilter<SingleEyeGazeData, EyeVelocity, VelocityCalculationOptions>("Eye velocity calculation", (s, o, _) => s.CalculateVelocities(o))
            ).Execute(args);
        }
    }
}
