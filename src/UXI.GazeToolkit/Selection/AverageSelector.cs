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
            bool hasLeftEye = gaze.LeftEye.Validity.HasEye();
            bool hasRightEye = gaze.RightEye.Validity.HasEye();

            if (hasLeftEye && hasRightEye)
            {
                return StrictAverageSelector.Instance.SelectSingleEye(gaze);
            }
            else if (hasLeftEye)
            {
                return LeftEyeSelector.Instance.SelectSingleEye(gaze);
            }
            else if (hasRightEye)
            {
                return RightEyeSelector.Instance.SelectSingleEye(gaze);
            }

            return new SingleEyeGazeData(EyeData.Default, gaze.TrackerTicks);
        }
    }
}
