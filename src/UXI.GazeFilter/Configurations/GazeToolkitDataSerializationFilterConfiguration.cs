using UXI.Filters;
using UXI.Filters.Configuration;
using UXI.GazeToolkit.Serialization.Csv;
using UXI.GazeToolkit.Serialization.Json;
using UXI.Serialization;
using UXI.Serialization.Extensions;

namespace UXI.GazeFilter.Configurations
{
    public class GazeToolkitDataSerializationFilterConfiguration : FilterConfiguration
    {
        public override void Configure(FilterContext context, object options)
        {
            context.IO
                   .Formats
                   .GetOrDefault(FileFormat.JSON)?
                   .Configurations
                   .Add
                   (
                        new JsonGazeToolkitDataConvertersSerializationConfiguration()
                   );

            context.IO
                   .Formats
                   .GetOrDefault(FileFormat.CSV)?
                   .Configurations
                   .Add
                   (
                       new CsvGazeToolkitDataConvertersSerializationConfiguration()
                   );
        }
    }
}
