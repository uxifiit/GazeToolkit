using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.Filters;
using UXI.GazeFilter;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Selection;

namespace UXI.GazeFilter.Selection
{
    public class EyeSelectionOptions : BaseOptions, IEyeSelectionOptions
    {
        [Option('s', "select", Default = EyeSelectionStrategy.Average, HelpText = "Which eye should be selected for the classification.")]
        public EyeSelectionStrategy EyeSelectionStrategy { get; set; }
    }



    static class Program
    {
        static int Main(string[] args)
        {
            return new SingleFilterHost<GazeFilterContext, EyeSelectionOptions>
            (
                new RelayFilter<GazeData, SingleEyeGazeData, EyeSelectionOptions>("Eye selection", (s, o, _) => s.SelectEye(o))
            ).Execute(args);
        }
    }
}
