using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Interpolation
{
    public static class LinearInterpolation
    {
        public static double Interpolate(double start, double end, int step, int totalSteps)
        {
            return GetStepSize(start, end, totalSteps) * step + start;
        }


        private static double GetStepSize(double start, double end, int totalSteps)
        {
            return (end - start) / totalSteps;
        }
    }
}
