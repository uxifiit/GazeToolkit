using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.GazeToolkit.Serialization;

namespace UXI.GazeFilter
{
    public abstract class BaseOptions
    {
        [Value(0, HelpText = "Path to the input file. If omitted, standard input stream is used.", MetaName = "input file", MetaValue = "FILE", Required = false)]
        public string InputFile { get; set; }


        [Option("format", Default = FileFormat.JSON, HelpText = "Data format of the input.")]
        public FileFormat InputFileFormat { get; set; }


        [Option("output-format", HelpText = "Data format of the output. If not specified, it is the same as input file format.", Required = false)]
        public FileFormat? OutputFileFormat { get; set; }


        [Option('o', "output", Default = null, HelpText = "Path to the output file. If not omitted, standard output stream is used.", Required = false)]
        public string OutputFile { get; set; }


        [Option('q', "quiet", Default = false, HelpText = "Suppress filter messages.", Required = false)]
        public bool SuppressMessages { get; set; }


        [Option("timestamp-format", HelpText = "Format of timestamps in data.", Required = false)]
        public string TimestampFormat { get; set; }


        [Option("timestamp-field", HelpText = "Name of the timestamp field in the data.", Required = false)]
        public string TimestampFieldName { get; set; }
    }
}
