using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
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
        static void Main(string[] args)
        {
            new FilterTool<GazeData, SingleEyeGazeData>
            (
                new Filter<GazeData, SingleEyeGazeData, EyeSelectionOptions>("Eye selection", (s, o) => s.SelectEye(o))
            ).Execute(args);
        }
    }
}
