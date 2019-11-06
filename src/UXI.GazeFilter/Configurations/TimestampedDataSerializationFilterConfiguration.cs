using UXI.Filters;
using UXI.Filters.Configuration;
using UXI.GazeToolkit.Serialization.Csv;
using UXI.GazeToolkit.Serialization.Json;
using UXI.Serialization;
using UXI.Serialization.Extensions;

namespace UXI.GazeFilter.Configurations
{
    public class TimestampedDataSerializationFilterConfiguration : FilterConfiguration<ITimestampedDataSerializationOptions>
    {
        protected override void ConfigureOverride(FilterContext context, ITimestampedDataSerializationOptions options)
        {
            string timestampFieldName = options.TimestampFieldName;

            context.IO
                   .Formats
                   .GetOrDefault(FileFormat.JSON)?
                   .Configurations
                   .Add
                   (
                       new JsonTimestampedDataSerializationConfiguration(timestampFieldName)
                   );

            context.IO
                   .Formats
                   .GetOrDefault(FileFormat.CSV)?
                   .Configurations
                   .Add
                   (
                       new CsvTimestampedDataSerializationConfiguration(timestampFieldName)
                   );
        }
    }
}
