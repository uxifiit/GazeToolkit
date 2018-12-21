using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Serialization;
using UXI.Serialization;

namespace UXI.GazeFilter.Statistics
{
    public class SamplesCounterStatistics : IFilterStatistics
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private int _inputSamplesCount = 0;
        private int _outputSamplesCount = 0;

        public SamplesCounterStatistics(string filterName)
        {
            FilterName = filterName;
            InputObserver = System.Reactive.Observer.Create<object>(_ => { _stopwatch.Start(); _inputSamplesCount += 1; });
            OutputObserver = System.Reactive.Observer.Create<object>(_ => _outputSamplesCount += 1, ex => Error(ex), () => _stopwatch.Stop());
        }

        public string FilterName { get; }

        public IObserver<object> InputObserver { get; }

        public IObserver<object> OutputObserver { get; }

        public Type DataType { get; } = typeof(SamplesCount);

        public FileFormat DefaultFormat => FileFormat.CSV;

        private void Error(Exception ex)
        {
            _stopwatch.Stop();
        }

        public IEnumerable<object> GetResults()
        {
            yield return new SamplesCount(FilterName, _inputSamplesCount, _outputSamplesCount, _stopwatch.Elapsed);
        }
    }
}
