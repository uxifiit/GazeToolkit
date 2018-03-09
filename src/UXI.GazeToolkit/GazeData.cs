using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    /// <summary>
    /// Class representing gaze data separately for both eyes, including validity code. Timestamp, when it was recorded, is saved both in <seealso cref="TrackerTicks"/> and <seealso cref="Timestamp"/> properties.
    /// </summary>
    public class GazeData : ITimestamped
    {
        public static readonly GazeData Empty = new GazeData(GazeDataValidity.None, EyeGazeData.Empty, EyeGazeData.Empty, 0, TimeSpan.MinValue);

        public GazeData(EyeGazeData leftEye, EyeGazeData rightEye, long trackerTicks, TimeSpan timestamp)
            : this(EyeGazeDataValidityEx.MergeToEyeValidity(leftEye.Validity, rightEye.Validity), leftEye, rightEye, trackerTicks, timestamp)
        {
        }

        public GazeData(GazeDataValidity validity, EyeGazeData leftEye, EyeGazeData rightEye, long trackerTicks, TimeSpan timestamp)
        {
            TrackerTicks = trackerTicks;
            Validity = validity;
            LeftEye = leftEye;
            RightEye = rightEye;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Time when data was sampled by the EyeTracker. Microseconds from arbitrary point in time.
        /// </summary>
        public long TrackerTicks { get; }

        public GazeDataValidity Validity { get; }

        public EyeGazeData LeftEye { get; }

        public EyeGazeData RightEye { get; }

        public TimeSpan Timestamp { get; } 
    }
}
