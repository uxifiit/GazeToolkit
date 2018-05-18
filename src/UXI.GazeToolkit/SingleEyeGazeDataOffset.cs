using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit
{
    public class SingleEyeGazeDataOffset : EyeGazeData
    {
        public SingleEyeGazeDataOffset()
            : this(EyeGazeDataValidity.Invalid, Point2.Default, Point3.Default, Point3.Default, Point3.Default, 0d, 0L)
        { }


        public SingleEyeGazeDataOffset
        (
            EyeGazeDataValidity validity,
            Point2 gazePoint2D,
            Point3 gazePoint3D,
            Point3 eyePosition3D,
            Point3 eyePosition3DRelative,
            double pupilDiameter,
            long timestampOffset
        )
            : base(validity, gazePoint2D, gazePoint3D, eyePosition3D, eyePosition3DRelative, pupilDiameter)
        {
            TimestampOffset = timestampOffset;
        }


        public SingleEyeGazeDataOffset(EyeGazeData data, long timestamp)
            : this
        (
            data.Validity,
            data.GazePoint2D,
            data.GazePoint3D,
            data.EyePosition3D,
            data.EyePosition3DRelative,
            data.PupilDiameter,
            timestamp
        )
        {
        }


        public SingleEyeGazeDataOffset(SingleEyeGazeData data)
            : this(data, data.Timestamp)
        {
        }




        public long TimestampOffset { get; }


        public SingleEyeGazeDataOffset Add(SingleEyeGazeDataOffset other)
        {
            return new SingleEyeGazeDataOffset
            (
                this.Validity,
                GazePoint2D + other.GazePoint2D,
                GazePoint3D + other.GazePoint3D,
                EyePosition3D + other.EyePosition3D,
                EyePosition3DRelative + other.EyePosition3DRelative,
                PupilDiameter + other.PupilDiameter,
                TimestampOffset + other.TimestampOffset
            );
        }


        public SingleEyeGazeDataOffset Subtract(SingleEyeGazeDataOffset other)
        {
            return new SingleEyeGazeDataOffset
            (
                this.Validity,
                GazePoint2D - other.GazePoint2D,
                GazePoint3D - other.GazePoint3D,
                EyePosition3D - other.EyePosition3D,
                EyePosition3DRelative - other.EyePosition3DRelative,
                PupilDiameter - other.PupilDiameter,
                TimestampOffset - other.TimestampOffset
            );
        }


        public SingleEyeGazeDataOffset Normalize(int count)
        {
            return new SingleEyeGazeDataOffset
            (
                this.Validity,
                GazePoint2D / count,
                GazePoint3D / count,
                EyePosition3D / count,
                EyePosition3DRelative / count,
                PupilDiameter / count,
                TimestampOffset / count
            );
        }


        public SingleEyeGazeData ToSingleEyeGazeData()
        {
            return new SingleEyeGazeData
            (
                 this,
                 TimestampOffset
            );
        }


        public static implicit operator SingleEyeGazeDataOffset(SingleEyeGazeData value)
        {
            return new SingleEyeGazeDataOffset(value);
        }
    }
}
