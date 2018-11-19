using System;
using System.Linq;
using System.Reactive.Linq;

namespace UXI.GazeFilter
{
    public class Filter<TSource, TResult, TOptions> : IFilter
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


        public Type InputType { get; } = typeof(TSource);


        public Type OutputType { get; } = typeof(TResult);


        public Type OptionsType { get; } = typeof(TOptions);


        public virtual void Initialize(FilterConfiguration configuration) { }


        public IObservable<object> Process(IObservable<object> data, object options)
        {
            return Process(data.OfType<TSource>(), (TOptions)options).Select(d => (object)d);
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
}
