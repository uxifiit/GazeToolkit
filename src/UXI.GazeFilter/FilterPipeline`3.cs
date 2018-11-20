using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Serialization;

namespace UXI.GazeFilter
{
    public class FilterPipeline<TInput, TOutput, TOptions> : IFilter
    {
        private readonly List<IFilter> _filters;

        public FilterPipeline(IEnumerable<IFilter> filters)
        {
            _filters = filters.ToList();
        }


        public FilterPipeline(params IFilter[] filters)
            : this(filters.AsEnumerable())
        {

        }


        public Type InputType { get; } = typeof(TInput);


        public Type OutputType { get; } = typeof(TOutput);


        public Type OptionsType { get; } = typeof(TOptions);


        public void Initialize(object options, SerializationConfiguration configuration, DataIO io)
        {
            // TODO add validate output-input connections in the pipeline

            foreach (var filter in _filters)
            {
                filter.Initialize(options, configuration, io);
            }
        }


        public IObservable<object> Process(IObservable<object> data, object options)
        {
            IObservable<object> pipeline = data.Publish().RefCount();

            foreach (var filter in _filters)
            {
                pipeline = filter.Process(pipeline, options);
            }

            return pipeline;
        }
    }
}
