using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeFilter.Exceptions;
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


        public void Initialize(object options, FilterContext context)
        {
            ValidateFilters();

            foreach (var filter in _filters)
            {
                filter.Initialize(options, context);
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


        private void ValidateFilters()
        {
            // ensure that the pipeline input type matches the input of the first filter
            EnsureCanConnect(this.GetType(), this.InputType, _filters.First().GetType(), _filters.First().InputType);
            // ensure that the pipeline output type matches the output of the last filter
            EnsureCanConnect(this.GetType(), this.OutputType, _filters.Last().GetType(), _filters.Last().OutputType);

            // then check output and input types pairwise through the pipeline
            // create pairs of successive filters
            var filterPairs = _filters.Zip(_filters.Skip(1), (a, b) => new Tuple<IFilter, IFilter>(a, b));

            // ensure their output and input type match
            foreach (var filterPair in filterPairs)
            {
                EnsureCanConnect(filterPair.Item1.GetType(), filterPair.Item1.OutputType, filterPair.Item2.GetType(), filterPair.Item2.InputType);
            }
        }


        private static void EnsureCanConnect(Type source, Type sourceDataType, Type target, Type targetDataType)
        {
            bool canAssign = sourceDataType == targetDataType
                          || sourceDataType.IsSubclassOf(targetDataType)
                          || targetDataType.IsAssignableFrom(sourceDataType);

            if (canAssign == false)
            {
                throw new FilterTypeMismatchException($"Mismatch in data types between filters [{source.FullName}] and [{target.FullName}]: [{sourceDataType.FullName}] does not match the expected type [{targetDataType.FullName}].");
            }
        }
    }
}
