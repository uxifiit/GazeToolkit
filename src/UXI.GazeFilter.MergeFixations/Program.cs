using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.GazeFilter;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Fixations;

namespace UXI.GazeFilter.MergeFixations
{
    public class FixationsMergingOptions : BaseOptions, IFixationsMergingOptions
    {
        [Option('g', "max-gap", Default = 75, HelpText = "Max time between fixations used when merging adjacent fixations.")]
        public double MaxTimeBetweenFixationsLength
        {
            get
            {
                return MaxTimeBetweenFixations.TotalMilliseconds;
            }
            set
            {
                MaxTimeBetweenFixations = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan MaxTimeBetweenFixations { get; private set; }


        [Option('a', "max-angle", Default = 0.5, HelpText = "Max angle between fixations used when merging adjacent fixations.")]
        public double MaxAngleBetweenFixations { get; set; }
    }


    static class Program
    {
        static int Main(string[] args)
        {
            return new SingleFilterHost<FixationsMergingOptions>
            (
                new RelayFilter<EyeMovement, EyeMovement, FixationsMergingOptions>("Merge adjacent fixations", (s, o) => s.MergeAdjacentFixations(o))
            ).Execute(args);
        }
    }
}
