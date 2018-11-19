using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter
{
    public class RelayFilter<TSource, TResult, TOptions> : Filter<TSource, TResult, TOptions>
        where TOptions : BaseOptions
    {
        private readonly Func<IObservable<TSource>, TOptions, IObservable<TResult>> _filter;

        protected RelayFilter()
            : base()
        {
            _filter = (_, __) => Observable.Empty<TResult>();
        }


        public RelayFilter(Func<IObservable<TSource>, TOptions, IObservable<TResult>> filter)
            : base()
        {
            _filter = filter;
        }


        public RelayFilter(string name, Func<IObservable<TSource>, TOptions, IObservable<TResult>> filter)
            : base(name)
        {
            _filter = filter;
        }


        protected override void Initialize(TOptions options) { }


        protected override IObservable<TResult> Process(IObservable<TSource> data, TOptions options)
        {
            return _filter.Invoke(data, options);
        }
    }
}
