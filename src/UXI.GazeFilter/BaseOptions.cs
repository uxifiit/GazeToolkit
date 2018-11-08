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
        [Value(0, HelpText = "Path to the input file.", MetaName = "input file", MetaValue = "FILE", Required = false)]
        public string InputFile { get; set; }

        [Option("format", Default = FileFormat.JSON, HelpText = "Data source format of the input file.")]
        public FileFormat InputFileType { get; set; }

        [Option("output-format", Default = FileFormat.JSON, HelpText = "Data format of the output.", Required = false)]
        public FileFormat OutputFileType { get; set; }

        [Option('o', "output", Default = null, HelpText = "Path to the output file.", Required = false)]
        public string OutputFile { get; set; }

        [Option('q', "quiet", Default = false, HelpText = "Suppress messages.", Required = false)]
        public bool SuppressMessages { get; set; }
    }
}
