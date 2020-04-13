using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit.Selection
{
    public sealed class LeftEyeSelector : ISingleEyeGazeDataSelector
    {
        private static readonly Lazy<LeftEyeSelector> _instance = new Lazy<LeftEyeSelector>();

        public static LeftEyeSelector Instance => _instance.Value;


        private LeftEyeSelector() { }


        public SingleEyeGazeData SelectSingleEye(GazeData gaze)
        {
            return new SingleEyeGazeData(gaze.LeftEye, gaze.Timestamp);
        }
    }
}
