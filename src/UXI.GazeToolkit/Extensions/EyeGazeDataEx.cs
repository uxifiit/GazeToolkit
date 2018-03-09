using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Utils;

namespace UXI.GazeToolkit.Extensions
{
    public static class EyeGazeDataEx
    {
        public static double GetVisualAngle(this EyeGazeData sample, EyeGazeData from, EyeGazeData to)
        {
            var euclidianDistance = PointsUtils.EuclidianDistance(from.GazePoint3D, to.GazePoint3D);
            var eyeToGazeDistanceFirst = PointsUtils.EuclidianDistance(from.GazePoint3D, sample.EyePosition3D); // Relative
            var eyeToGazeDistanceLast = PointsUtils.EuclidianDistance(to.GazePoint3D, sample.EyePosition3D);

            var visualAngle = MathUtils.RadianToDegree
                              (
                                  Math.Acos
                                  (
                                      (Math.Pow(eyeToGazeDistanceLast, 2) + Math.Pow(eyeToGazeDistanceFirst, 2) - Math.Pow(euclidianDistance, 2))
                                      / 
                                      (2 * eyeToGazeDistanceFirst * eyeToGazeDistanceLast)
                                  )
                              );

            return visualAngle;
        }
    }
}
