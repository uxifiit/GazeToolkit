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


        public static IObservable<int> MeasureFrequency(this IObservable<long> data)
        {
            return MeasureFrequency(data, DefaultWindow);
        }


        public static IObservable<int> MeasureFrequency(this IObservable<ITimestampedData> data)
        {
            return MeasureFrequency(data.Select(d => d.TrackerTicks));
        }


        public static IObservable<int> MeasureFrequency(this IObservable<long> data, TimeSpan timeWindow)
        {
            return data.Buffer((first, current) => (current - first) * 10 >= timeWindow.Ticks)
                       .Select(buffer => buffer.Count);
        }


        public static IObservable<int> MeasureFrequency(this IObservable<ITimestampedData> data, TimeSpan timeWindow)
        {
            return MeasureFrequency(data.Select(d => d.TrackerTicks), timeWindow);
        }


        public static IObservable<int> MeasureFrequency(this IObservable<long> data, IFrequencyMeasureOptions options)
        {
            return MeasureFrequency(data, options.TimeWindow);
        }


        public static IObservable<int> MeasureFrequency(this IObservable<ITimestampedData> data, IFrequencyMeasureOptions options)
        {
            return MeasureFrequency(data.Select(d => d.TrackerTicks), options);
        }
    }
}
