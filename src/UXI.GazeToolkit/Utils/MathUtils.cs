using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Utils
{
    public static class MathUtils
    {
        private static double GetStepSize(double start, double end, int totalSteps)
        {
            return (end - start) / totalSteps; 
        }


        public static double Interpolate(double start, double end, int step, int totalSteps)
        {
            return GetStepSize(start, end, totalSteps) * step + start;
        }


        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180d;
        }


        public static double RadianToDegree(double angle)
        {
            return angle * (180d / Math.PI);
        }
    }
}
