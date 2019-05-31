using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.Filters;
using UXI.GazeFilter;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Interpolation;

namespace UXI.GazeFilter.FillInGaps
{
    public class FillInGapsOptions : BaseOptions, IFillInGapsOptions
    {
        [Option('g', "max-gap", Default = 75, HelpText = "Interpolate data in case of missing or invalid data, if the gap in data is less or equal the max gap duration.", Required = false)]
        public double MaxGapDurationMilliseconds
        {
            get
            {
                return MaxGapDuration.TotalMilliseconds;
            }
            set
            {
                MaxGapDuration = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan MaxGapDuration { get; private set; }
    }



    static class Program
    {
        static int Main(string[] args)
        {
            return new SingleFilterHost<GazeFilterContext, FillInGapsOptions>
            (   
                new RelayFilter<GazeData, GazeData, FillInGapsOptions>("Fill in gaps", (s, o, _) => s.FillInGaps(o))
            ).Execute(args);
        }
    }
}
