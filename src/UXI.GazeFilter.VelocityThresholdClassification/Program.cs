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
        static void Main(string[] args)
        {
            new FilterTool<EyeVelocity, EyeMovement>
            (
                new Filter<EyeVelocity, EyeMovement, VelocityThresholdClassificationOptions>("Velocity threshold classification", (s, o) => s.ClassifyByVelocity(o))
            ).Execute(args);
        }
    }
}
