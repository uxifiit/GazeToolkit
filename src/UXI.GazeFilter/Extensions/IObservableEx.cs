using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter.Extensions
{
    public static class IObservableEx
    {
        public static IObservable<T> Attach<T>(this IObservable<T> source, IEnumerable<IObserver<T>> observers)
        {
            var result = source;

            foreach (var observer in observers)
            {
                result = result.Do(observer);
            }

            return result;
        }
    }
}
