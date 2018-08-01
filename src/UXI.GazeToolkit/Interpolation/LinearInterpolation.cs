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


    public static class PointsEx
    {
        public static Point2 Interpolate(int index, Point2 start, Point2 end, int count)
        {
            var x = LinearInterpolation.Interpolate(start.X, end.X, index, count);
            var y = LinearInterpolation.Interpolate(start.Y, end.Y, index, count);

            return new Point2(x, y);
        }


        public static Point3 Interpolate(int index, Point3 start, Point3 end, int count)
        {
            var x = LinearInterpolation.Interpolate(start.X, end.X, index, count);
            var y = LinearInterpolation.Interpolate(start.Y, end.Y, index, count);
            var z = LinearInterpolation.Interpolate(start.Z, end.Z, index, count);

            return new Point3(x, y, z);
        }
    }
}
