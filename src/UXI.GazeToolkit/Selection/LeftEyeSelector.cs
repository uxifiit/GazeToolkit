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
        private static readonly LeftEyeSelector _instance = new LeftEyeSelector();
        public static LeftEyeSelector Instance => _instance;


        private LeftEyeSelector() { }


        public SingleEyeGazeData SelectSingleEye(GazeData gaze)
        {
            return new SingleEyeGazeData(gaze.LeftEye, gaze.TrackerTicks, gaze.Timestamp);
        }
    }
}
