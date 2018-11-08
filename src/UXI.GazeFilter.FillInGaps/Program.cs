using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.GazeFilter;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Interpolation;

namespace UXI.GazeFilter.FillInGaps
{
    public class FillInGapsOptions : BaseOptions, IFillInGapsOptions
    {
        [Option('g', "max-gap", Default = 75, HelpText = "Interpolate data in case of missing or invalid data, if the gap in data is less or equal the max gap length.", Required = false)]
        public double MaxGapLength
        {
            get
            {
                return MaxGap.TotalMilliseconds;
            }
            set
            {
                MaxGap = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan MaxGap { get; private set; }
    }


    static class Program
    {
        static int Main(string[] args)
        {
            return new SingleFilterHost<FillInGapsOptions>
            (   
                new Filter<GazeData, GazeData, FillInGapsOptions>("Fill in gaps", (s, o) => s.FillInGaps(o))
            ).Execute(args);
        }
    }
}
