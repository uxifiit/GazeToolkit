using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit
{
    public class SingleEyeGazeData : EyeData, ITimestampedData
    {
        public SingleEyeGazeData
        (
            EyeValidity validity,
            Point2 gazePoint2D,
            Point3 gazePoint3D,
            Point3 eyePosition3D,
            Point3 eyePosition3DRelative,
            double pupilDiameter,
            long trackerTicks,
            TimeSpan timestamp
        )
            : base(validity, gazePoint2D, gazePoint3D, eyePosition3D, eyePosition3DRelative, pupilDiameter)
        {
            TrackerTicks = trackerTicks;
            Timestamp = timestamp;
        }


        public SingleEyeGazeData(EyeData other, long trackerTicks, TimeSpan timestamp)
            : base(other)
        {
            TrackerTicks = trackerTicks;
            Timestamp = timestamp;
        }


        public long TrackerTicks { get; }


        public TimeSpan Timestamp { get; }
    }
}
