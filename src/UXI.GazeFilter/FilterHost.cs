using CommandLine;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UXI.GazeFilter
{
    public abstract class FilterHost
    {
        private readonly StringWriter _help;
        private readonly Parser _commandLineParser;

        public FilterHost(IEnumerable<IFilter> filters)
        {
            if (filters == null || filters.Any() == false)
            {
                throw new ArgumentNullException(nameof(filters), "No filter specified");
            }

            _help = new StringWriter();
            _commandLineParser = new Parser(s =>
            {
                s.CaseSensitive = false;
                s.CaseInsensitiveEnumValues = true;
                s.ParsingCulture = CultureInfo.GetCultureInfo("en-US");
                s.HelpWriter = _help;
            });

            Filters = filters.ToDictionary(f => f.OptionsType, f => f);
        }


        public Dictionary<Type, IFilter> Filters { get; }


        public int Execute(string[] args)
        {
            BaseOptions options;
            IFilter filter;

            if (TryParseFilterOptions(_commandLineParser, args, out options)
                && Filters.TryGetValue(options.GetType(), out filter))
            {
#if DEBUG
                Console.Error.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(options, Newtonsoft.Json.Formatting.Indented, new StringEnumConverter(false)));
#endif

                using (var cts = new CancellationTokenSource())
                {
                    var execution = ExecuteAsync(filter, options, cts.Token);

                    Console.CancelKeyPress += (_, __) => cts.Cancel();

                    execution.Wait();
                }

                return 0;
            }
            else
            {
                Console.Error.WriteLine(_help.ToString());

                return 1;
            }
        }


        protected abstract bool TryParseFilterOptions(Parser parser, string[] args, out BaseOptions options);


        //protected virtual bool TryResolveFilter(BaseOptions options, out IFilter filter)
        //{
        //    filter = Filters.FirstOrDefault(f => f.OptionsType.Equals(options.GetType()));
        //    return filter != null;
        //}


        private Task ExecuteAsync(IFilter filter, BaseOptions options, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();

            FilterIO.ReadInput(options, filter.InputType)
                    .SubscribeOn(NewThreadScheduler.Default)
                    .Process(filter, options)
                    .WriteOutput(options, filter.OutputType)
                    .Subscribe(_ => { }, e => tcs.TrySetException(e), () => tcs.TrySetResult(true));

            return tcs.Task;
        }
    }

}
