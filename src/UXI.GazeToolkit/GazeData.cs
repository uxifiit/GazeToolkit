using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    /// <summary>
    /// Class representing gaze data separately for both eyes,
    /// including validity code and the timestamp when it was recorded (in microseconds).
    /// </summary>
    public class GazeData : ITimestampedData
    {
        public static readonly GazeData Empty = new GazeData(EyeData.Default, EyeData.Default, 0, TimeSpan.Zero);

        public GazeData(EyeData leftEye, EyeData rightEye, long trackerTicks, TimeSpan timestamp)
        {
            LeftEye = leftEye;
            RightEye = rightEye;
            TrackerTicks = trackerTicks;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Time when the data was sampled by the Eye Tracker in microseconds from arbitrary point in time. 
        /// </summary>
        public long TrackerTicks { get; }

        /// <summary>
        /// Time when the data was received from or sampled by the Eye Tracker. 
        /// </summary>
        public TimeSpan Timestamp { get; }

        public EyeData LeftEye { get; }

        public EyeData RightEye { get; }
    }
}
