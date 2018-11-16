using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter
{
    public class FilterStatistics<TSource, TResult>
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        public FilterStatistics(string filterName)
        {
            FilterName = filterName;
            InputObserver = System.Reactive.Observer.Create<TSource>(_ => { _stopwatch.Start(); InputSamplesCount += 1; });
            OutputObserver = System.Reactive.Observer.Create<TResult>(_ => OutputSamplesCount += 1, ex => Error(ex), () => Stop());
        }

        public string FilterName { get; }

        public int InputSamplesCount { get; private set; } = 0;

        public IObserver<TSource> InputObserver { get; }

        public int OutputSamplesCount { get; private set; } = 0;

        public IObserver<TResult> OutputObserver { get; }

        private void Stop()
        {
            _stopwatch.Stop();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{FilterName}");
            sb.AppendLine($"- Input samples: {InputSamplesCount}");
            sb.AppendLine($"- Output samples: {OutputSamplesCount}");
            sb.AppendLine($"- Runtime: {_stopwatch.Elapsed.ToString()}");

            Console.Error.WriteLine(sb.ToString());
        }

        private void Error(Exception ex)
        {
            Stop();
            Console.Error.WriteLine(ex);
        }
    }
}
