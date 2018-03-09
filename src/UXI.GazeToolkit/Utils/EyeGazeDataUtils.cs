using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Utils
{
    public static class EyeGazeDataUtils
    {
        public static EyeGazeData Average(EyeGazeData first, EyeGazeData second)
        {
            return new EyeGazeData
            (
                EyeGazeDataValidity.Valid,
                PointsUtils.Average(first.GazePoint2D, second.GazePoint2D),
                PointsUtils.Average(first.GazePoint3D, second.GazePoint3D),
                PointsUtils.Average(first.EyePosition3D, second.EyePosition3D),
                PointsUtils.Average(first.EyePosition3DRelative, second.EyePosition3DRelative),
                (first.PupilDiameter + second.PupilDiameter) / 2
            );
        }
    }
}
