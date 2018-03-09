using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace UXI.GazeFilter
{
    public abstract class BaseOptions
    {
        [Value(0, HelpText = "Path to the input file with gaze data.", MetaName = "input file", MetaValue = "FILE", Required = false)]
        public string InputFile { get; set; }

        [Option("format", Default = InputFileType.JSON, HelpText = "Data source format of the input file.")]
        public InputFileType InputFileType { get; set; }

        [Option("output-format", Default = OutputFileType.JSON, HelpText = "Data format of the output.", Required = false)]
        public OutputFileType OutputFileType { get; set; }

        [Option('o', "output", Default = null, HelpText = "Path to the file for the output.", Required = false)]
        public string OutputFile { get; set; }
    }
}
