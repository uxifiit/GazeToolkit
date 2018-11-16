using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Extensions;

namespace UXI.GazeToolkit.Utils
{
    public static class EyeSampleUtils
    {
        public static EyeSample Average(EyeSample first, EyeSample second)
        {
            return new EyeSample
            (
                PointsUtils.Average(first.GazePoint2D, second.GazePoint2D),
                PointsUtils.Average(first.GazePoint3D, second.GazePoint3D),
                PointsUtils.Average(first.EyePosition3D, second.EyePosition3D),
                (first.PupilDiameter + second.PupilDiameter) / 2
            );
        }


        public static EyeSample Average(IEnumerable<EyeSample> data)
        {
            return data.Count() <= 1  
                 ? data.FirstOrDefault()
                 : new EyeSample
                   (
                       PointsUtils.Average(data.Select(s => s.GazePoint2D)),
                       PointsUtils.Average(data.Select(s => s.GazePoint3D)),
                       PointsUtils.Average(data.Select(s => s.EyePosition3D)),
                       MathUtils.Average(data.Select(s => s.PupilDiameter))
                   );
        }


        public static double GetVisualAngle(Point3 eyePosition, Point3 fromGazePoint, Point3 toGazePoint)
        {
            // create gaze vectors with the origin in eye position of the sample
            var fromVector = PointsUtils.Vectors.GetNormalizedVector(eyePosition, fromGazePoint);
            var toVector = PointsUtils.Vectors.GetNormalizedVector(eyePosition, toGazePoint);

            // visual angle in radians
            var angleRad = PointsUtils.Vectors.GetAngle(fromVector, toVector);

            // convert radians to degrees
            var angleDeg = MathUtils.ConvertRadToDeg(angleRad);

            // convert angle to positive value
            return MathUtils.WrapAround(angleDeg, 360);
        }
    }
}
