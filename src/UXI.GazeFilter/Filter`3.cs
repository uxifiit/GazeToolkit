using System;
using System.Linq;
using System.Reactive.Linq;

namespace UXI.GazeFilter
{
    public abstract class Filter<TSource, TResult, TOptions> : IFilter
        where TOptions : BaseOptions
    {
        private readonly FilterStatistics<TSource, TResult> _statistics;

        protected Filter() { }

        protected Filter(string name)
        {
            _statistics = new FilterStatistics<TSource, TResult>(name);
        }


        public Type InputType { get; } = typeof(TSource);


        public Type OutputType { get; } = typeof(TResult);


        public Type OptionsType { get; } = typeof(TOptions);


        public void Initialize(object options)
        {
            if (options is TOptions)
            {
                Initialize((TOptions)options);
            }
        }


        protected abstract void Initialize(TOptions options);


        public IObservable<object> Process(IObservable<object> data, object options)
        {
            var baseOptions = options as BaseOptions;

            var typedData = data.OfType<TSource>();

            if (baseOptions != null && baseOptions.SuppressMessages == false && _statistics != null)
            {
                typedData = typedData.Do(_statistics.InputObserver);
            }

            var publishedData = typedData.Publish().RefCount();

            var result = Process(publishedData, (TOptions)options);

            if (baseOptions != null && baseOptions.SuppressMessages == false && _statistics != null)
            {
                result = result.Do(_statistics.OutputObserver);
            }

            return result.Select(d => (object)d);
        }


        protected abstract IObservable<TResult> Process(IObservable<TSource> data, TOptions options);
    }

    
}
