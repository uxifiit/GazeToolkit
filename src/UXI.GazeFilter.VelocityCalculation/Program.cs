using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.GazeFilter;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Fixations.VelocityThreshold;

namespace UXI.GazeFilter.VelocityCalculation
{
    public class VelocityCalculationOptions : BaseOptions, IVelocityCalculationOptions
    {
        [Option('f', "frequency", Default = null, SetName = "frequency", HelpText = "Frequency of the eye tracking data.")]
        public int? DataFrequency { get; set; }


        [Option('w', "window-side", Default = VelocityCalculationRx.DefaultTimeWindowSideMilliseconds, SetName = "frequency", HelpText = "Time window length in milliseconds to use for measuring frequency of the eye tracking data")]
        public double TimeWindowSideLength
        {
            get
            {
                return TimeWindowSide.TotalMilliseconds;
            }
            set
            {
                TimeWindowSide = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan TimeWindowSide { get; private set; }
    }


    static class Program
    {
        static void Main(string[] args)
        {
            new FilterTool<SingleEyeGazeData, EyeVelocity>
            (
                new Filter<SingleEyeGazeData, EyeVelocity, VelocityCalculationOptions>("Eye velocity calculation", (s, o) => s.CalculateVelocities(o))
            ).Execute(args);
        }
    }
}
