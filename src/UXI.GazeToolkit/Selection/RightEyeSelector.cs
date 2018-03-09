using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit.Selection
{
    public sealed class RightEyeSelector : ISingleEyeGazeDataSelector
    {
        private static readonly RightEyeSelector _instance = new RightEyeSelector();
        public static RightEyeSelector Instance => _instance;


        private RightEyeSelector() { }


        public SingleEyeGazeData SelectSingleEye(GazeData gaze)
        {
            return new SingleEyeGazeData(gaze.RightEye, gaze.TrackerTicks, gaze.Timestamp);
        }
    }
}
