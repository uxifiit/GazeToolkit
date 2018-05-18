using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit
{
    public class SingleEyeGazeData : EyeGazeData
    {
        public SingleEyeGazeData
        (
            EyeGazeDataValidity validity,
            Point2 gazePoint2D,
            Point3 gazePoint3D,
            Point3 eyePosition3D,
            Point3 eyePosition3DRelative,
            double pupilDiameter,
            long timestamp
        )
            : base(validity, gazePoint2D, gazePoint3D, eyePosition3D, eyePosition3DRelative, pupilDiameter)
        {
            Timestamp = timestamp;
        }


        public SingleEyeGazeData(EyeGazeData eyeGazeData, long timestamp)
            : this
            (
                eyeGazeData.Validity,
                eyeGazeData.GazePoint2D,
                eyeGazeData.GazePoint3D,
                eyeGazeData.EyePosition3D,
                eyeGazeData.EyePosition3DRelative,
                eyeGazeData.PupilDiameter,
                timestamp
            )
        {
        }


        public long Timestamp { get; }


        public static SingleEyeGazeData Average(params SingleEyeGazeData[] data)
        {
            if (data == null || data.Any() == false)
            {
                throw new ArgumentException("No data to average.", nameof(data));
            }

            return AverageRange(data);
        }


        public static SingleEyeGazeData AverageRange(IEnumerable<SingleEyeGazeData> data)
        {
            var reference = new SingleEyeGazeDataOffset(data.First());
            var aggregate = new SingleEyeGazeDataOffset();
            var rest = data.Skip(1);
            int count = 1;

            foreach (var gaze in rest)
            {
                aggregate = aggregate.Add(new SingleEyeGazeDataOffset(gaze).Subtract(reference));
                count += 1;
            }

            var referenceOffset = aggregate.Normalize(count);

            var average = reference.Add(referenceOffset);

            return average.ToSingleEyeGazeData();
        }
    }
}
