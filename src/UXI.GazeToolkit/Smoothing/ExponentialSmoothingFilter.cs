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
            if (alpha < 0d || alpha > 1d)
            {
                throw new ArgumentOutOfRangeException(nameof(alpha), "Alpha value must be on the interval from 0 to 1, inclusive.");
            }

            Alpha = alpha;
        }


        public double Alpha { get; }


        public IObservable<SingleEyeGazeData> Smooth(IObservable<SingleEyeGazeData> gazeData)
        {
            return gazeData.Scan((previous, current) =>
            {
                return (current.Validity == EyeValidity.Valid && previous.Validity == EyeValidity.Valid)
                ? new SingleEyeGazeData
                  (
                      EyeValidity.Valid,
                      Smooth(previous.GazePoint2D, current.GazePoint2D),
                      Smooth(previous.GazePoint3D, current.GazePoint3D),
                      Smooth(previous.EyePosition3D, current.EyePosition3D),
                      current.PupilDiameter,
                      current.TrackerTicks
                  )
                : current;
            });
        }


        private Point2 Smooth(Point2 previous, Point2 current)
        {
            return new Point2
            (
                ExponentialSmoothing.Smooth(previous.X, current.X, Alpha),
                ExponentialSmoothing.Smooth(previous.Y, current.Y, Alpha)
            );
        }


        private Point3 Smooth(Point3 previous, Point3 current)
        {
            return new Point3
            (
                ExponentialSmoothing.Smooth(previous.X, current.X, Alpha),
                ExponentialSmoothing.Smooth(previous.Y, current.Y, Alpha),
                ExponentialSmoothing.Smooth(previous.Z, current.Z, Alpha)
            );
        }
    }
}
