using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Smoothing
{
    public class MedianSmoothingFilter : ISingleEyeGazeDataSmoothingFilter
    {
        public IObservable<SingleEyeGazeData> Smooth(IObservable<SingleEyeGazeData> source)
        {
            return null;
        }
    }
}
