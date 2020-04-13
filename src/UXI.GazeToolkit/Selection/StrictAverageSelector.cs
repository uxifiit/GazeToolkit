using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Extensions;
using UXI.GazeToolkit.Utils;

namespace UXI.GazeToolkit.Selection
{
    public sealed class StrictAverageSelector : ISingleEyeGazeDataSelector
    {
        private static readonly Lazy<StrictAverageSelector> _instance = new Lazy<StrictAverageSelector>();

        public static StrictAverageSelector Instance => _instance.Value;


        private StrictAverageSelector() { }


        public SingleEyeGazeData SelectSingleEye(GazeData gaze)
        {
            var average = (gaze.LeftEye.Validity == EyeValidity.Valid && gaze.RightEye.Validity == EyeValidity.Valid)
                          ? new EyeData(EyeValidity.Valid, EyeSampleUtils.Average(gaze.LeftEye, gaze.RightEye))
                          : EyeData.Default;

            return new SingleEyeGazeData(average, gaze.Timestamp);
        }
    }
}
