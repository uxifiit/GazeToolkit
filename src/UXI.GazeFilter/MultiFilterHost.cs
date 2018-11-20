using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter
{
    public class MultiFilterHost : FilterHost
    {
        public MultiFilterHost(params IFilter[] filters)
            : base(filters.AsEnumerable())
        {

        }


        public MultiFilterHost(Action<FilterConfiguration> configure, params IFilter[] filters)
            : base(configure, filters.AsEnumerable())
        {

        }
        

        protected override bool TryParseFilterOptions(Parser parser, string[] args, out BaseOptions options)
        {
            bool parsed = false;
            BaseOptions parsedOptions = null;

            parser.ParseArguments(args, Filters.Keys.ToArray()).WithParsed(o =>
            {
                parsedOptions = (BaseOptions)o;
                parsed = true;
            });

            options = parsedOptions;
            return parsed;
        }
    }
}
