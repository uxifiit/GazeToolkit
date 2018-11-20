using System;
using System.Linq;
using System.Reactive.Linq;
using UXI.GazeToolkit.Serialization;

namespace UXI.GazeFilter
{
    public abstract class Filter<TInput, TOutput, TOptions> : IFilter
    {
        private readonly FilterStatistics<TInput, TOutput> _statistics;

        protected Filter() { }

        protected Filter(string name)
        {
            _statistics = new FilterStatistics<TInput, TOutput>(name);
        }


        public Type InputType { get; } = typeof(TInput);


        public Type OutputType { get; } = typeof(TOutput);


        public Type OptionsType { get; } = typeof(TOptions);


        public void Initialize(object options, SerializationConfiguration configuration, DataIO io)
        {
            if (options is TOptions)
            {
                Initialize((TOptions)options, configuration, io);
            }
        }


        protected abstract void Initialize(TOptions options, SerializationConfiguration configuration, DataIO io);


        public IObservable<object> Process(IObservable<object> data, object options)
        {
            var baseOptions = options as BaseOptions;

            var typedData = data.OfType<TInput>();

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


        protected abstract IObservable<TOutput> Process(IObservable<TInput> data, TOptions options);
    }
}
