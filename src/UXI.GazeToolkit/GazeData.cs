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
    public class GazeData : ITimestampedData
    {
        public static readonly GazeData Empty = new GazeData(EyeGazeData.Empty, EyeGazeData.Empty, 0, TimeSpan.Zero);

        public GazeData(EyeGazeData leftEye, EyeGazeData rightEye, long trackerTicks, TimeSpan timestamp)
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

        // TODO TIMESTAMP
        /// <summary>
        /// Time when the data was received from or sampled by the Eye Tracker. 
        /// </summary>
        public TimeSpan Timestamp { get; }

        public EyeGazeData LeftEye { get; }

        public EyeGazeData RightEye { get; }
    }
}
