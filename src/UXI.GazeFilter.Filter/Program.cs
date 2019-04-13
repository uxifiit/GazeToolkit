using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.GazeToolkit;
using UXI.Serialization.Configurations;

namespace UXI.GazeFilter.Filter
{
    public abstract class BaseFilterOptions : BaseOptions
    {
        [Option("json-indent", Default = false, HelpText = "Use to indent JSON output.", Required = false)]
        public bool IndentJsonOutput { get; set; }


        [Option("timestamp-from", Default = null, HelpText = "Minimum timestamp of the data. Must be in the same format as specified with the --timestamp-format option.", Required = false)]
        public string FromTimestampString { get; set; }


        public DateTimeOffset? FromTimestamp { get; set; }


        [Option("timestamp-to", Default = null, HelpText = "Maximum timestamp of the data. Must be in the same format as specified with the --timestamp-format option.", Required = false)]
        public string ToTimestampString { get; set; }


        public DateTimeOffset? ToTimestamp { get; set; }
    }



    [Verb("gaze")]
    public class FilterGazeDataOptions : BaseFilterOptions { }



    [Verb("movement")]
    public class FilterEyeMovementOptions : BaseFilterOptions { }



    [Verb("single-eye")]
    public class FilterSingleEyeGazeDataOptions : BaseFilterOptions { }



    static class Program
    {
        static int Main(string[] args)
        {
            return new MultiFilterHost
            (
                Configure,
                new RelayFilter<GazeData, GazeData, FilterGazeDataOptions>("Filter Gaze Data", (s, o) => FilterByTimestamps(s, o)),
                new RelayFilter<EyeMovement, EyeMovement, FilterEyeMovementOptions>("Filter Eye Movement", (s, o) => FilterByTimestamps(s, o)),
                new RelayFilter<SingleEyeGazeData, SingleEyeGazeData, FilterSingleEyeGazeDataOptions>("Filter Single Eye Gaze Data", (s, o) => FilterByTimestamps(s, o))
            ).Execute(args);
        }


        private static IObservable<T> FilterByTimestamps<T>(IObservable<T> data, BaseFilterOptions options)
            where T : ITimestampedData
        {
            var result = data;

            if (options.FromTimestamp.HasValue)
            {
                result = result.SkipWhile(d => d.Timestamp < options.FromTimestamp.Value);
            }

            if (options.ToTimestamp.HasValue)
            {
                result = result.TakeWhile(d => d.Timestamp < options.ToTimestamp.Value);
            }

            return result;
        }


        private static void Configure(FilterContext context, BaseOptions options)
        {
            var filterOptions = (BaseFilterOptions)options;
            if (filterOptions.IndentJsonOutput)
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

            var timestampConverter = context.Serialization.TimestampConverter;

            if (String.IsNullOrWhiteSpace(filterOptions.FromTimestampString) == false)
            {
                filterOptions.FromTimestamp = timestampConverter.Convert(filterOptions.FromTimestampString);
            }

            if (String.IsNullOrWhiteSpace(filterOptions.ToTimestampString) == false)
            {
                filterOptions.ToTimestamp = timestampConverter.Convert(filterOptions.ToTimestampString);
            }
        }
    }
}
