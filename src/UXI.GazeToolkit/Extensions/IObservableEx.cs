using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Extensions
{
    public static class IObservableEx
    {
        public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, Func<T, bool> closeBufferPredicate)
        {
            return Observable.Create<IList<T>>(
                observer =>
                {
                    var closings = new Subject<Unit>();
                    return source.Do(v =>
                                 {
                                     if (closeBufferPredicate.Invoke(v))
                                     {
                                         closings.OnNext(Unit.Default);
                                     }
                                 })
                                 .Buffer(() => closings)
                                 .Subscribe(observer);
                });
        }


        public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, Func<T, T, bool> closeBufferPredicate)
        {
            return Observable.Create<IList<T>>(
                observer =>
                {
                    bool isFirst = true;
                    T firstValue = default(T);
                    var closings = new Subject<Unit>();

                    return source.Do(value =>
                                 {
                                     if (isFirst)
                                     {
                                         isFirst = false;
                                         firstValue = value;
                                     }   
                                     else if (closeBufferPredicate.Invoke(firstValue, value))
                                     {
                                         isFirst = false;
                                         firstValue = value;
                                         closings.OnNext(Unit.Default);
                                     }
                                 })
                                 .Buffer(() => closings)
                                 .Subscribe(observer);
                });
        }


        //public static IObservable<IList<T>> ContinuousBuffer<T>(this IObservable<T> source, Func<T, T, bool> closeBufferPredicate)
        //{
        //    return Observable.Create<IList<T>>(
        //        observer =>
        //        {
        //            bool isFirst = true;
        //            T previousValue = default(T);
        //            var closings = new Subject<Unit>();

        //            return source.Do(value =>
        //            {
        //                if (isFirst)
        //                {
        //                    isFirst = false;
        //                    previousValue = value;
        //                }
        //                else if (closeBufferPredicate.Invoke(previousValue, value))
        //                {
        //                    isFirst = false;
        //                    previousValue = value;
        //                    closings.OnNext(Unit.Default);
        //                }
        //                else
        //                {
        //                    previousValue = value;
        //                }
        //            })
        //                         .Buffer(() => closings)
        //                         .Subscribe(observer);
        //        });
        //}


        public static IObservable<TResult> MovingAverage<TItem, TAccumulate, TResult>
        (
            this IObservable<TItem> source, 
            TAccumulate seed, 
            int windowSize, 
            Func<TAccumulate, TItem, TAccumulate> accumulateWindowEnd, 
            Func<TAccumulate, TAccumulate, int, TItem, TResult> average
        )
        {
            var windowsBegin = source.Scan(seed, accumulateWindowEnd);
            var windowsEnd = windowsBegin.Skip(windowSize * 2 + 1);

            var itemsToAverage = source.Skip(windowSize + 1);

            return Observable.Zip(windowsBegin, windowsEnd, itemsToAverage, (begin, end, item) => average(begin, end, windowSize * 2 + 1, item));  
        }


        // TODO refactor with single scan, without buffer
        [Obsolete("Do not use, current implementation is not optimal and possibly buggy.")]
        public static IObservable<TResult> MovingAverageWithBuffer<TItem, TAccumulate, TResult>
        (
            this IObservable<TItem> source, 
            TAccumulate seed, 
            int windowSize,
            Func<TAccumulate, TItem, TAccumulate> addAccumulate, 
            Func<TAccumulate, TItem, TAccumulate> subtractAccumulate,
            Func<TAccumulate, int, TItem, TResult> average
        )
        {
            return Observable.Create<TResult>(observer =>
            {
                return source.Buffer(windowSize * 2 + 1, 1)
                             .Scan
                             (
                                 seed: new MovingAverageBuffer<TAccumulate, TItem>
                                 (
                                     accumulate: seed,
                                     add: addAccumulate,
                                     subtract: subtractAccumulate
                                 ),
                                 accumulator: (aggregate, buffer) =>
                                 {
                                     if (aggregate.IsInitialized == false)
                                     {
                                         aggregate.Initialize(buffer);
                                     }
                                     else if (buffer.Count < aggregate.Count)
                                     {
                                         aggregate.First = buffer.First();
                                     }
                                     else
                                     {
                                         aggregate.Last = buffer.Last();
                                         aggregate.First = buffer.First();
                                     }
 
                                     int count = buffer.Count;
 
                                     //if (count != aggregate.Count)
                                     //{
                                     //    Console.WriteLine("xxxx");
                                     //}
 
                                     if (count % 2 == 1)
                                     {
                                         var current = buffer[count / 2];
 
                                         observer.OnNext(average(aggregate.Accumulate, count, current));
                                     }
 
                                     return aggregate;
                                 }
                             ).Subscribe(_ => { }, () => observer.OnCompleted());
            }); 
        }


        private class MovingAverageBuffer<TAccumulate, TItem>
        {
            private readonly Func<TAccumulate, TItem, TAccumulate> _add;
            private readonly Func<TAccumulate, TItem, TAccumulate> _subtract;
            private readonly EqualityComparer<TItem> _comparer = EqualityComparer<TItem>.Default;


            public MovingAverageBuffer(TAccumulate accumulate, Func<TAccumulate, TItem, TAccumulate> add, Func<TAccumulate, TItem, TAccumulate> subtract)
            {
                Accumulate = accumulate;
                _add = add;
                _subtract = subtract;
            }


            public TAccumulate Accumulate { get; private set; }


            public bool IsInitialized { get; private set; } = false;
            public void Initialize(IEnumerable<TItem> items)
            {
                if (IsInitialized == false)
                {
                    IsInitialized = true;

                    foreach (var item in items)
                    {
                        Last = item;
                    }
                }
            }


            public int Count { get; private set; } = 0;


            private TItem last;
            public TItem Last
            {
                get { return last; }
                set
                {
                    last = value;
                    Accumulate = _add(Accumulate, value);

                    if (_comparer.Equals(first, default(TItem)))
                    {
                        first = value;
                    }

                    Count += 1;
                }
            }


            private TItem first;
            public TItem First
            {
                get { return first; }
                set
                {
                    var previous = first;
                    first = value;

                    if (_comparer.Equals(previous, default(TItem)) == false)
                    {
                        Accumulate = _subtract(Accumulate, previous);
                        Count -= 1;
                    }
                }
            }
        }
    }
}
