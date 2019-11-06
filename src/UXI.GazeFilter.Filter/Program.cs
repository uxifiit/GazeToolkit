using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.Filters;
using UXI.Filters.Configuration;
using UXI.Filters.Serialization.Converters;
using UXI.GazeToolkit;

namespace UXI.GazeFilter.Filter
{
    public abstract class BaseFilterOptions : BaseOptions
    {
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
            return new MultiFilterHost<GazeFilterContext>
            (
                new FilterConfiguration[] 
                {
                    new RelayFilterConfiguration<BaseFilterOptions>(Configure)
                },
                new RelayFilter<GazeData, GazeData, FilterGazeDataOptions>("Filter Gaze Data", (s, o, _) => FilterByTimestamps(s, o)),
                new RelayFilter<EyeMovement, EyeMovement, FilterEyeMovementOptions>("Filter Eye Movement", (s, o, _) => FilterByTimestamps(s, o)),
                new RelayFilter<SingleEyeGazeData, SingleEyeGazeData, FilterSingleEyeGazeDataOptions>("Filter Single Eye Gaze Data", (s, o, _) => FilterByTimestamps(s, o))
            ).Execute(args);
        }


        private static void Configure(FilterContext context, BaseFilterOptions options)
        {
            ITimestampStringConverter timestampConverter = TimestampStringConverterResolver.Default.Resolve(options.TimestampFormat);

            if (String.IsNullOrWhiteSpace(options.FromTimestampString) == false)
            {
                options.FromTimestamp = timestampConverter.Convert(options.FromTimestampString);
            }

            if (String.IsNullOrWhiteSpace(options.ToTimestampString) == false)
            {
                options.ToTimestamp = timestampConverter.Convert(options.ToTimestampString);
            }
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
    }
}
