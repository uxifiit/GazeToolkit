using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter.Fakes
{
    public class NullFilter<TInput, TOutput, TOptions> : IFilter
    {
        public Type InputType { get; } = typeof(TInput);

        public Type OptionsType { get; } = typeof(TOptions);

        public Type OutputType { get; } = typeof(TOutput);

        public void Initialize(object options, FilterContext context)
        {
        }

        public IObservable<object> Process(IObservable<object> data, object options)
        {
            return data;
        }
    }

}
