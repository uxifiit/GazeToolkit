using System;
using System.Linq;
using System.Reactive.Linq;
using UXI.GazeToolkit.Serialization;

namespace UXI.GazeFilter
{
    public abstract class Filter<TInput, TOutput, TOptions> : IFilter
    {

        protected Filter() { }

        protected Filter(string name)
        {
            Name = name;
        }


        public string Name { get;  }


        public Type InputType { get; } = typeof(TInput);


        public Type OutputType { get; } = typeof(TOutput);


        public Type OptionsType { get; } = typeof(TOptions);


        public void Initialize(object options, FilterContext context)
        {
            if (options is TOptions)
            {
                Initialize((TOptions)options, context);
            }
        }


        protected abstract void Initialize(TOptions options, FilterContext context);


        public IObservable<object> Process(IObservable<object> data, object options)
        {
            var baseOptions = options as BaseOptions;

            var typedData = data.OfType<TInput>();

            var result = Process(typedData, (TOptions)options);

            return result.Select(d => (object)d);
        }


        protected abstract IObservable<TOutput> Process(IObservable<TInput> data, TOptions options);
    }
}
