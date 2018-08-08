using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Selection;

namespace UXI.GazeToolkit.Extensions
{
    public static class ISingleEyeGazeDataSelectorEx
    {
        public static IEnumerable<SingleEyeGazeData> SelectSingleEye(this ISingleEyeGazeDataSelector selector, IEnumerable<GazeData> gazeData)
        {
            return gazeData.Select(g => selector.SelectSingleEye(g));
        }


        public static IObservable<SingleEyeGazeData> SelectSingleEye(this ISingleEyeGazeDataSelector selector, IObservable<GazeData> gazeData)
        {
            return gazeData.Select(g => selector.SelectSingleEye(g));
        }
    }
}
