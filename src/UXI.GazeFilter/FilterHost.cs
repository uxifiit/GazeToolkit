using CommandLine;
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
using UXI.GazeToolkit.Serialization;
using UXI.GazeToolkit.Serialization.Converters;

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

            Filters = filters.ToDictionary(f => f.OptionsType);

            Configuration = new FilterConfiguration();           
        }

        
        public FilterHost(Action<FilterConfiguration> configure, IEnumerable<IFilter> filters)
            : this(filters)
        {
            configure?.Invoke(Configuration);
        }


        public Dictionary<Type, IFilter> Filters { get; }


        public FilterConfiguration Configuration { get; }


        public int Execute(string[] args)
        {
            BaseOptions options;
            IFilter filter;

            if (TryParseFilterOptions(_commandLineParser, args, out options)
                && Filters.TryGetValue(options.GetType(), out filter))
            {
//#if DEBUG
//                Console.Error.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(options, Newtonsoft.Json.Formatting.Indented, new StringEnumConverter(false)));
//#endif
                Configure(options);

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
        

        private Task ExecuteAsync(IFilter filter, BaseOptions options, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();

            var io = new DataIO(Configuration.Formats);

            filter.Initialize(options, Configuration.Serialization, io);

            io.ReadInput(options, filter.InputType, Configuration.Serialization)
              .SubscribeOn(NewThreadScheduler.Default)
              .Process(filter, options)
              .WriteOutput(io, options, filter.OutputType, Configuration.Serialization)
              .Subscribe(_ => { }, e => tcs.TrySetException(e), () => tcs.TrySetResult(true));

            return tcs.Task;
        }


        private void Configure(BaseOptions options)
        {
            Configuration.Serialization.TimestampConverter = TimestampStringConverterResolver.Default.Resolve(options.TimestampFormat);
            Configuration.Serialization.TimestampFieldName = options.TimestampFieldName;
        }
    }
}
