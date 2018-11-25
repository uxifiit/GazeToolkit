using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter.Statistics
{
    public class SamplesCount
    {
        public SamplesCount(string filterName, int inputSamples, int outputSamples, TimeSpan runtime)
        {
            FilterName = filterName;
            InputSamples = inputSamples;
            OutputSamples = outputSamples;
            Runtime = runtime;
        }

        public string FilterName { get; }
                                       
        public int InputSamples { get; }
                                       
        public int OutputSamples { get; }
                                       
        public TimeSpan Runtime { get; }
    }
}
