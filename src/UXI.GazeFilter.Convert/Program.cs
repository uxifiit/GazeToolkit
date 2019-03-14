using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.GazeToolkit;
using UXI.Serialization.Configurations;

namespace UXI.GazeFilter.Convert
{
    public abstract class BaseConvertOptions : BaseOptions
    {
        [Option('i', "indent", Default = false, HelpText = "Use to indent JSON output.", Required = false)]
        public bool Indent { get; set; }

        
        public string BeginTimestamp { get; set; }

        public string EndTimestamp { get; set; }
    }


    [Verb("gaze")]
    public class ConvertGazeDataOptions : BaseConvertOptions
    {
    }



    static class Program
    {
        static int Main(string[] args)
        {
            return new MultiFilterHost
            (
                Configure,
                new RelayFilter<GazeData, GazeData, ConvertGazeDataOptions>("Convert Gaze Data", (s, o) => s)
            ).Execute(args);
        }

        private static void Configure(FilterContext context, BaseOptions options)
        {
            var convertOptions = (BaseConvertOptions)options;
            if (convertOptions.Indent)
            {
                context.Formats
                       .FirstOrDefault(f => f.Format == Serialization.FileFormat.JSON)?
                       .Configurations
                       .Add(new RelaySerializationConfiguration<Newtonsoft.Json.JsonSerializer>((serializer, access, _) =>
                       {
                           serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

                           return serializer;
                       }));
            }
        }
    }
}
