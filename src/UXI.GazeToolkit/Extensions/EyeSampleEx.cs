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
            // create gaze vectors with the origin in eye position of the sample
            var fromVector = PointsUtils.Vectors.GetNormalizedVector(sample.EyePosition3DRelative, from.GazePoint3D);
            var toVector = PointsUtils.Vectors.GetNormalizedVector(sample.EyePosition3DRelative, to.GazePoint3D);

            // visual angle in radians
            var angleRad = PointsUtils.Vectors.GetAngle(fromVector, toVector);

            // clamp to <-1,1> interval
            angleRad = Math.Min(1, Math.Max(-1, angleRad));

            // convert radians to degrees
            var angleDeg = MathUtils.RadianToDegree(angleRad);

            // convert angle to positive value
            return (angleDeg + 360) % 360;
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
