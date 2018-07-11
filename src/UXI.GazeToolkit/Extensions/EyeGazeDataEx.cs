using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Utils;

namespace UXI.GazeToolkit.Extensions
{
    public static class EyeSampleEx
    {
        public static double GetVisualAngle(this EyeSample sample, EyeSample from, EyeSample to)
        {
            return GetAngle(sample.EyePosition3D, from.GazePoint3D, to.GazePoint3D);
        }


        public static double GetAngle(this Point3 origin, Point3 from, Point3 to)
        {
            var euclidianDistance = PointsUtils.EuclidianDistance(from, to);
            var eyeToGazeDistanceFirst = PointsUtils.EuclidianDistance(from, origin);
            var eyeToGazeDistanceLast = PointsUtils.EuclidianDistance(to, origin);

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


        public static EyeSample Add(this EyeSample current, EyeSampleAggregate aggregate)
        {
            return new EyeSample
            (
                current.GazePoint2D + aggregate.GazePoint2D,
                current.GazePoint3D + aggregate.GazePoint3D,
                current.EyePosition3D + aggregate.EyePosition3D,
                current.EyePosition3DRelative + aggregate.EyePosition3DRelative,
                current.PupilDiameter + aggregate.PupilDiameter
            );
        }


        public static EyeSample Subtract(this EyeSample current, EyeSampleAggregate aggregate)
        {
            return new EyeSample
            (
                current.GazePoint2D - aggregate.GazePoint2D,
                current.GazePoint3D - aggregate.GazePoint3D,
                current.EyePosition3D - aggregate.EyePosition3D,
                current.EyePosition3DRelative - aggregate.EyePosition3DRelative,
                current.PupilDiameter - aggregate.PupilDiameter
            );
        }
    }
}
