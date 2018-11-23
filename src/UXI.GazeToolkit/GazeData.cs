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
        public static readonly GazeData Empty = new GazeData(EyeData.Default, EyeData.Default, DateTimeOffset.MinValue);


        public GazeData(EyeData leftEye, EyeData rightEye, DateTimeOffset timestamp)
        {
            LeftEye = leftEye;
            RightEye = rightEye;
            Timestamp = timestamp;
        }


        /// <summary>
        /// Time when the data was sampled by the Eye Tracker in microseconds from arbitrary point in time. 
        /// </summary>
        public DateTimeOffset Timestamp { get; }


        public EyeData LeftEye { get; }


        public EyeData RightEye { get; }
    }
}
