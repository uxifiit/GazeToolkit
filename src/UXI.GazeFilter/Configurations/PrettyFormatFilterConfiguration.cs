using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Filters;
using UXI.Filters.Configuration;
using UXI.Filters.Options;

namespace UXI.GazeFilter.Configurations
{
    public class PrettyFormatFilterConfiguration : FilterConfiguration<IPrettyFormatOptions>
    {
        private readonly FilterConfiguration[] _configurations = new FilterConfiguration[]
        {
            new IndentedJsonOutputFormattingConfiguration()
        };

        protected override void ConfigureOverride(FilterContext context, IPrettyFormatOptions options)
        {
            if (options.IsPrettyFormatEnabled)
            {
                foreach (var configuration in _configurations)
                {
                    configuration.Configure(context, options);
                }
            }
        }
    }
}
