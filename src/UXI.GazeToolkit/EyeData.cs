using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public class EyeData : EyeSample
    {
        public static readonly EyeData Default = new EyeData(EyeValidity.Invalid, EyeSample.Empty);

        public EyeData
        (
            EyeValidity validity,
            Point2 gazePoint2D,
            Point3 gazePoint3D,
            Point3 eyePosition3D,
            Point3 eyePosition3DRelative,
            double pupilDiameter
        )
            : base(gazePoint2D, gazePoint3D, eyePosition3D, eyePosition3DRelative, pupilDiameter)
        {
            Validity = validity;
        }


        public EyeData(EyeValidity validity, EyeSample other)
            : base(other)
        {
            Validity = validity;
        }


        public EyeData(EyeData other)
            : this(other.Validity, other)
        {
        }


        public EyeValidity Validity { get; } = EyeValidity.Invalid;
    }
}
