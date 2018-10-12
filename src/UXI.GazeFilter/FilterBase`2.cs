using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Newtonsoft.Json.Converters;
using UXI.GazeFilter.Extensions;

namespace UXI.GazeFilter
{
    public interface IFilter<TSource, TResult>
    {
        Type OptionsType { get; } 
        IObservable<TResult> Process(IObservable<TSource> data, object options);
    }


    public class FilterStatistics<TSource, TResult>
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        public FilterStatistics(string filterName)
        {
            FilterName = filterName;
            InputObserver = System.Reactive.Observer.Create<TSource>(_ => { _stopwatch.Start(); InputSamplesCount += 1; });
            OutputObserver = System.Reactive.Observer.Create<TResult>(_ => OutputSamplesCount += 1, ex => Error(ex), () => Stop());
        }

        public string FilterName { get; }

        public int InputSamplesCount { get; private set; } = 0;

        public IObserver<TSource> InputObserver { get; }

        public int OutputSamplesCount { get; private set; } = 0;

        public IObserver<TResult> OutputObserver { get; }

        private void Stop()
        {
            _stopwatch.Stop();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{FilterName}");
            sb.AppendLine($"- Input samples: {InputSamplesCount}");
            sb.AppendLine($"- Output samples: {OutputSamplesCount}");
            sb.AppendLine($"- Runtime: {_stopwatch.Elapsed.ToString()}");

            Console.Error.WriteLine(sb.ToString());
        }

        private void Error(Exception ex)
        {
            Stop();
            Console.Error.WriteLine(ex);
        }
    }


    public class Filter<TSource, TResult, TOptions> : IFilter<TSource, TResult>
        where TOptions : BaseOptions
    {
        private readonly Func<IObservable<TSource>, TOptions, IObservable<TResult>> _filter;
        private readonly FilterStatistics<TSource, TResult> _statistics;

        protected Filter()
        {
            _filter = (_, __) => Observable.Empty<TResult>();
        }

        public Filter(string name, Func<IObservable<TSource>, TOptions, IObservable<TResult>> filter)
        {
            _statistics = new FilterStatistics<TSource, TResult>(name);
            _filter = filter;
        }


        public Type OptionsType { get; } = typeof(TOptions);


        public IObservable<TResult> Process(IObservable<TSource> data, object options)
        {
            return Process(data, (TOptions)options);
        }


        protected virtual IObservable<TResult> Process(IObservable<TSource> data, TOptions options)
        {
            if (options.SuppressMessages == false && _statistics != null)
            {
                data = data.Do(_statistics.InputObserver);
            }

            var publishedData = data.Publish().RefCount();

            var result = _filter.Invoke(publishedData, options);

            if (options.SuppressMessages == false && _statistics != null)
            {
                result = result.Do(_statistics.OutputObserver);
            }

            return result;
        }
    }


    internal static class FilterEx
    {
        internal static IObservable<TResult> Process<TSource, TResult>(this IObservable<TSource> data, IFilter<TSource, TResult> filter, object options)
        {
            return filter.Process(data, options); 
        }
    }


    public class FilterTool<TSource, TResult>
    {
        private readonly Parser _commandLineParser;
        private readonly bool _usesVerbs;

        public FilterTool(bool usesVerbs, params IFilter<TSource, TResult>[] filters)
            : this(filters)
        {
            _usesVerbs = usesVerbs;
        }


        public FilterTool(params IFilter<TSource, TResult>[] filters)
            : this(filters.AsEnumerable())
        {
        }


        public FilterTool(IEnumerable<IFilter<TSource, TResult>> filters)
        {
            _commandLineParser = new Parser(s =>
            {
                s.CaseSensitive = false;
                s.ParsingCulture = CultureInfo.GetCultureInfo("en-US");
                s.CaseInsensitiveEnumValues = true;
            });
            Filters = filters;
        }


        public IEnumerable<IFilter<TSource, TResult>> Filters { get; }


        public void Execute(string[] args)
        {
            using (var cts = new CancellationTokenSource())
            {
                var execution = ExecuteAsync(args, cts.Token);

                Console.CancelKeyPress += (_, __) => cts.Cancel();

                execution.Wait();
            }
        }


        private Task ExecuteAsync(string[] args, CancellationToken cancellationToken)
        {
            IFilter<TSource, TResult> filter = null;
            object options = null;

            bool resolved = (_usesVerbs == false && TryResolveFilterForArguments(args, out filter, out options))
                            || (_usesVerbs && TryResolveFilterForArgumentsVerbs(args, out filter, out options));
                
            if (resolved)
            {
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

#if DEBUG
                Console.Error.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(options, Newtonsoft.Json.Formatting.Indented, new StringEnumConverter(false)));
#endif

                FilterIO.ReadInput<TSource>((BaseOptions)options)
                        .SubscribeOn(NewThreadScheduler.Default)
                        .Process(filter, options)
                        .WriteOutput((BaseOptions)options)
                        .Subscribe(_ => { }, e => tcs.TrySetException(e), () => tcs.TrySetResult(true));

                return tcs.Task;
            }

            return Task.FromResult(false);
        }


        private bool TryResolveFilterForArguments(string[] args, out IFilter<TSource, TResult> matchingFilter, out object options)
        {
            foreach (var filter in Filters)
            {
                if (TryParseOptionsFromArguments(args, filter.OptionsType, out options))
                {
                    matchingFilter = filter;
                    return true;
                }
            }

            matchingFilter = null;
            options = null;
            return false;
        }


        private bool TryParseOptionsFromArguments(string[] args, Type optionsType, out object options)
        {
            bool parsed = false;
            object parsedOptions = null;

            _commandLineParser.ParseArguments(args, optionsType, (o) =>
            {
                parsed = true;
                parsedOptions = o;
            }, errors => { });

            options = parsedOptions;
            return parsed;
        }


        private bool TryResolveFilterForArgumentsVerbs(string[] args, out IFilter<TSource, TResult> matchingFilter, out object options)
        {
            var result = _commandLineParser.ParseArguments(args, Filters.Select(f => f.OptionsType).ToArray());

            foreach (var filter in Filters)
            {
                if (TryMatchArgumentsVerb(result, filter.OptionsType, out options))
                {
                    matchingFilter = filter;
                    return true;
                }   
            }

            matchingFilter = null;
            options = null;
            return false;
        }


        private bool TryMatchArgumentsVerb(ParserResult<object> result, Type optionsType, out object options)
        {
            bool parsed = false;
            object parsedOptions = null;

            result.WithParsedExactly(optionsType, (o) =>
            {
                parsed = true;
                parsedOptions = o;
            });

            options = parsedOptions;
            return parsed;
        }


        private void PrintError(IEnumerable<Error> errors)
        {
            var helpText = new HelpText();
            //var helpText = HelpText.AutoBuild(options);
            //helpText.AddEnumValuesToHelpText = true;
            //helpText.AddOptions(options);
            Console.Error.Write(helpText);
            Environment.Exit(1);
        }
    }
}
