using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Extensions;

namespace UXI.GazeToolkit.Selection
{
    public sealed class AverageSelector : ISingleEyeGazeDataSelector
    {
        private static readonly AverageSelector _instance = new AverageSelector();
        public static AverageSelector Instance => _instance;


        private AverageSelector() { }


        public SingleEyeGazeData SelectSingleEye(GazeData gaze)
        {
            if (gaze.Validity == GazeDataValidity.Both)
            {
                return StrictAverageSelector.Instance.SelectSingleEye(gaze);
            }
            else if (gaze.Validity.HasLeftEye())
            {
                return LeftEyeSelector.Instance.SelectSingleEye(gaze);
            }
            else if (gaze.Validity.HasRightEye())
            {
                return RightEyeSelector.Instance.SelectSingleEye(gaze);
            }

            return new SingleEyeGazeData(EyeGazeData.Empty, gaze.TrackerTicks, gaze.Timestamp);
        }
    }
}
