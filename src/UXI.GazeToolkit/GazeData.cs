using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    /// <summary>
    /// Class representing gaze data separately for both eyes, including validity code and the timestamp, when it was recorded, in microseconds.
    /// </summary>
    public class GazeData : ITimestamped
    {
        public static readonly GazeData Empty = new GazeData(GazeDataValidity.None, EyeGazeData.Empty, EyeGazeData.Empty, 0);

        public GazeData(EyeGazeData leftEye, EyeGazeData rightEye, long timestamp)
            : this(EyeGazeDataValidityEx.MergeToEyeValidity(leftEye.Validity, rightEye.Validity), leftEye, rightEye, timestamp)
        {
        }

        public GazeData(GazeDataValidity validity, EyeGazeData leftEye, EyeGazeData rightEye, long timestamp)
        {
            Timestamp = timestamp;
            Validity = validity;
            LeftEye = leftEye;
            RightEye = rightEye;
        }

        /// <summary>
        /// Time when data was sampled by the EyeTracker in microseconds from arbitrary point in time.
        /// </summary>
        public long Timestamp { get; }

        public GazeDataValidity Validity { get; }

        public EyeGazeData LeftEye { get; }

        public EyeGazeData RightEye { get; }
    }
}
