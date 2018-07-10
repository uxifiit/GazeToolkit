using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit.Smoothing
{
    public class ExponentialSmoothingFilter : ISingleEyeGazeDataSmoothingFilter
    {
        public const double DEFAULT_ALPHA = 0.3d;

        public ExponentialSmoothingFilter()
            : this(DEFAULT_ALPHA)
        {
        }


        public ExponentialSmoothingFilter(double alpha)
        {
            Alpha = alpha;
        }


        public double Alpha { get; }


        public IObservable<SingleEyeGazeData> Smooth(IObservable<SingleEyeGazeData> gazeData)
        {
            return gazeData.Scan((previous, current) =>
            {
                return (current.Validity == EyeGazeDataValidity.Valid && previous.Validity == EyeGazeDataValidity.Valid)
                ? new SingleEyeGazeData
                  (
                      EyeGazeDataValidity.Valid,
                      Smooth(previous.GazePoint2D, current.GazePoint2D),
                      Smooth(previous.GazePoint3D, current.GazePoint3D),
                      Smooth(previous.EyePosition3D, current.EyePosition3D),
                      Smooth(previous.EyePosition3DRelative, current.EyePosition3DRelative),
                      current.PupilDiameter,
                      current.TrackerTicks,
                      current.Timestamp
                  )
                : current;
            });
        }


        private Point2 Smooth(Point2 previous, Point2 current) => new Point2
        (
            ExponentialSmoothing.Smooth(previous.X, current.X, Alpha),
            ExponentialSmoothing.Smooth(previous.Y, current.Y, Alpha)
        );


        private Point3 Smooth(Point3 previous, Point3 current) => new Point3
        (
            ExponentialSmoothing.Smooth(previous.X, current.X, Alpha),
            ExponentialSmoothing.Smooth(previous.Y, current.Y, Alpha),
            ExponentialSmoothing.Smooth(previous.Z, current.Z, Alpha)
        );
    }
}
