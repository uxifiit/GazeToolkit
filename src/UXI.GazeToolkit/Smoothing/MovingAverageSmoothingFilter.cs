using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Extensions;

namespace UXI.GazeToolkit.Smoothing
{
    public class MovingAverageSmoothingFilter : ISingleEyeGazeDataSmoothingFilter
    {
        public const int DEFAULT_WINDOW_SIZE = 10;

        public MovingAverageSmoothingFilter()
            : this(DEFAULT_WINDOW_SIZE)
        {
        }


        public MovingAverageSmoothingFilter(int windowSize)
        {
            WindowSize = windowSize;
        }


        public int WindowSize { get; }


        public IObservable<SingleEyeGazeData> Smooth(IObservable<SingleEyeGazeData> gazeData)
        {
            return gazeData.MovingAverageWithBuffer(
                new EyeSampleAggregate(),
                WindowSize,
                (accumulate, eye) => accumulate.Add(eye),
                (accumulate, eye) => accumulate.Subtract(eye),
                (accumulate, count, current) => new SingleEyeGazeData
                (
                    EyeValidity.Valid,
                    accumulate.GazePoint2D / count,
                    accumulate.GazePoint3D / count,
                    accumulate.EyePosition3D / count,
                    current.PupilDiameter,
                    current.TrackerTicks
                ));

            //return gazeData.MovingAverage(new SingleEyeGazeDataAggregate(), WindowSize, (accumulate, eye) => accumulate.Add(eye), (begin, end, count, current) =>

            //    new SingleEyeGazeData
            //        (
            //            SingleEyeValidity.Valid,
            //            new EyeGazeData
            //            (
            //                (end.GazePoint2D - begin.GazePoint2D)/ count,
            //                (end.GazePoint3D - begin.GazePoint3D) / count,
            //                (end.EyePosition3D - begin.EyePosition3D) / count,
            //                (end.EyePosition3DRelative - begin.EyePosition3DRelative) / count,
            //                current.PupilDiameter
            //            ),
            //            current.TrackerTicks,
            //            current.Timestamp
            //        ));
        }
    }
}
