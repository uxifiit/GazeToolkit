using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.GazeFilter;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Fixations;
using UXI.GazeToolkit.Fixations.VelocityThreshold;

namespace UXI.GazeFilter.DiscardFixations
{
    public class FixationsDiscardingOptions : BaseOptions, IFixationsDiscardingOptions
    {
        [Option('d', "min-duration", Default = 60, HelpText = "Minimum fixation duration.")]
        public double MinimumFixationDurationLength
        {
            get
            {
                return MinimumFixationDuration.TotalMilliseconds;
            }
            set
            {
                MinimumFixationDuration = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan MinimumFixationDuration { get; private set; }
    }


    static class Program
    {
        static int Main(string[] args)
        {
            return new SingleFilterHost<FixationsDiscardingOptions>
            (
                new Filter<EyeMovement, EyeMovement, FixationsDiscardingOptions>("Discard short fixations", (s, o) => s.DiscardShortFixations(o))
            ).Execute(args);
        }
    }
}
