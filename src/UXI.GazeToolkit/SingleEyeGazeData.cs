using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit
{
    public class SingleEyeGazeData : EyeGazeData, ITimestampedData
    {
        public SingleEyeGazeData
        (
            EyeGazeDataValidity validity,
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


        public SingleEyeGazeData(EyeGazeData eyeGazeData, long trackerTicks, TimeSpan timestamp)
            : this
            (
                eyeGazeData.Validity,
                eyeGazeData.GazePoint2D,
                eyeGazeData.GazePoint3D,
                eyeGazeData.EyePosition3D,
                eyeGazeData.EyePosition3DRelative,
                eyeGazeData.PupilDiameter,
                trackerTicks,
                timestamp
            )
        {
        }


        public long TrackerTicks { get; }


        // TODO Timestamp
        public TimeSpan Timestamp { get; }
    }
}
