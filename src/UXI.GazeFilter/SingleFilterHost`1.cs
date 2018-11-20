using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter
{
    public class SingleFilterHost<TOptions> : FilterHost
          where TOptions : BaseOptions
    {
        public SingleFilterHost(IFilter filter)
            : base(new[] { filter })
        {
            AssertOptionsTypeMatch(filter.OptionsType);
        }


        public SingleFilterHost(Action<FilterConfiguration> configure, IFilter filter)
            : base(configure, new[] { filter })
        {
            AssertOptionsTypeMatch(filter.OptionsType);
        }


        private void AssertOptionsTypeMatch(Type optionsType)
        {
            if (optionsType.Equals(typeof(TOptions)) == false)
            {
                throw new ArgumentException("Filter options type mismatch.");
            }
        }


        protected override bool TryParseFilterOptions(Parser parser, string[] args, out BaseOptions options)
        {
            bool parsed = false;
            TOptions parsedOptions = null;

            parser.ParseArguments<TOptions>(args).WithParsed(o =>
            {
                parsedOptions = o;
                parsed = true;
            });

            options = parsedOptions;
            return parsed;
        }
    }
}
