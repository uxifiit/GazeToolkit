using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Extensions;

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

    
        public static EyeGazeData Average(IEnumerable<EyeGazeData> data)
        {
            var reference = data.First();
            var aggregate = new EyeGazeDataAggregate();

            var rest = data.Skip(1);
            int count = 1;

            foreach (var gaze in rest)
            {
                aggregate.Add(gaze.Subtract(reference));
                count++;
            }

            aggregate.Normalize(count);

            EyeGazeData average = reference.Add(aggregate);

            return average;
        }
    }
}
