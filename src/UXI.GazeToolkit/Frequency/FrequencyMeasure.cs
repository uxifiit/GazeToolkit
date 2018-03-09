using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Extensions;

namespace UXI.GazeToolkit.Frequency
{
    public interface IFrequencyMeasureOptions
    {
        TimeSpan TimeWindow { get; }
    }


    public static class FrequencyMeasureRx
    {
        public static readonly TimeSpan DefaultWindow = TimeSpan.FromSeconds(1d);


        public static IObservable<int> MeasureFrequency(this IObservable<TimeSpan> data)
        {
            return MeasureFrequency(data, DefaultWindow);
        }


        public static IObservable<int> MeasureFrequency(this IObservable<ITimestamped> data)
        {
            return MeasureFrequency(data.Select(d => d.Timestamp));
        }


        public static IObservable<int> MeasureFrequency(this IObservable<TimeSpan> data, TimeSpan timeWindow)
        {
            return data.Buffer((first, current) => (current - first) >= timeWindow)
                       .Select(buffer => buffer.Count);
        }


        public static IObservable<int> MeasureFrequency(this IObservable<ITimestamped> data, TimeSpan timeWindow)
        {
            return MeasureFrequency(data.Select(d => d.Timestamp), timeWindow);
        }


        public static IObservable<int> MeasureFrequency(this IObservable<TimeSpan> data, IFrequencyMeasureOptions options)
        {
            return MeasureFrequency(data, options.TimeWindow);
        }


        public static IObservable<int> MeasureFrequency(this IObservable<ITimestamped> data, IFrequencyMeasureOptions options)
        {
            return MeasureFrequency(data.Select(d => d.Timestamp), options);
        }
    }
}
