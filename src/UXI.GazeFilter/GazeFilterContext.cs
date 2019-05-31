using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Filters;
using UXI.Filters.Configuration;
using UXI.GazeFilter.Configurations;

namespace UXI.GazeFilter
{
    [FilterConfiguration(typeof(TimestampSerializationConfiguration))]
    [FilterConfiguration(typeof(TimestampedDataSerializationFilterConfiguration))]
    [FilterConfiguration(typeof(GazeToolkitDataSerializationFilterConfiguration))]
    [FilterConfiguration(typeof(PrettyFormatFilterConfiguration))]
    public class GazeFilterContext : FilterContext
    {
    }
}
