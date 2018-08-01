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
                PointsUtils.Average(first.EyePosition3DRelative, second.EyePosition3DRelative),
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
                       PointsUtils.Average(data.Select(s => s.EyePosition3DRelative)),
                       MathUtils.Average(data.Select(s => s.PupilDiameter))
                   );
        }
    }
}
