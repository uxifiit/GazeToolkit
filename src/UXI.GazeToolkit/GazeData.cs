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
        public static readonly GazeData Empty = new GazeData(EyeData.Default, EyeData.Default, 0L);


        public GazeData(EyeData leftEye, EyeData rightEye, long trackerTicks)
        {
            LeftEye = leftEye;
            RightEye = rightEye;
            TrackerTicks = trackerTicks;
        }


        /// <summary>
        /// Time when the data was sampled by the Eye Tracker in microseconds from arbitrary point in time. 
        /// </summary>
        public long TrackerTicks { get; }


        public EyeData LeftEye { get; }


        public EyeData RightEye { get; }
    }
}
