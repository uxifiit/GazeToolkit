using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Filters;
using UXI.Filters.Configuration;
using UXI.GazeToolkit.Validation.Serialization.Csv;
using UXI.GazeToolkit.Validation.Serialization.Json;
using UXI.Serialization;
using UXI.Serialization.Extensions;

namespace UXI.GazeFilter.Validation.Serialization
{
    class ValidationDataSerializationFilterConfiguration : FilterConfiguration
    {
        public override void Configure(FilterContext context, object options)
        {
            context.IO
                   .Formats
                   .GetOrDefault(FileFormat.JSON)?
                   .Configurations
                   .Add(new JsonValidationDataConvertersSerializationConfiguration());

            context.IO
                   .Formats
                   .GetOrDefault(FileFormat.CSV)?
                   .Configurations
                   .Add(new CsvValidationDataConvertersSerializationConfiguration());
        }
    }
}
