using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.Filters;
using UXI.GazeFilter;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Frequency;
using UXI.GazeToolkit.Selection;

namespace UXI.GazeFilter.Frequency
{
    public class FrequencyMeasureOptions : BaseOptions, IFrequencyMeasureOptions
    {
        [Option('m', "measure-frequency-window", Default = 1000d, SetName = "frequency", HelpText = "Time window length in milliseconds to use for measuring frequency of the eye tracking data")]
        public double TimeWindowMilliseconds
        {
            get
            {
                return TimeWindow.TotalMilliseconds;
            }
            set
            {
                TimeWindow = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan TimeWindow { get; private set; }
    }



    static class Program
    {
        static int Main(string[] args)
        {
            return new SingleFilterHost<GazeFilterContext, FrequencyMeasureOptions>
            (
                new RelayFilter<ITimestampedData, int, FrequencyMeasureOptions>("Frequency measure", (s, o, _) => s.MeasureFrequency(o))
            ).Execute(args);
        }
    }
}
