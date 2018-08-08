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
            return EyeSampleUtils.GetVisualAngle(sample.EyePosition3D, from.GazePoint3D, to.GazePoint3D);
        }
    }
}
