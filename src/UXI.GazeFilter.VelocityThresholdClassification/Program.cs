using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.GazeFilter;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Fixations.VelocityThreshold;

namespace UXI.GazeFilter.VelocityThresholdClassification
{
    public class VelocityThresholdClassificationOptions : BaseOptions, IVelocityThresholdClassificationOptions
    {
        [Option('t', "threshold", Default = 30, HelpText = "Fixation movement classification threshold.")]
        public double VelocityThreshold { get; set; }
    }


    static class Program
    {
        static int Main(string[] args)
        {
            return new SingleFilterHost<VelocityThresholdClassificationOptions>
            (
                new RelayFilter<EyeVelocity, EyeMovement, VelocityThresholdClassificationOptions>("Velocity threshold classification", (s, o) => s.ClassifyByVelocity(o))
            ).Execute(args);
        }
    }
}
