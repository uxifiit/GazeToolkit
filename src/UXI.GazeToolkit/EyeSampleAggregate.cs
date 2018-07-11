using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit
{
    public class EyeSampleAggregate
    {
        public EyeSampleAggregate() { }

        public EyeSampleAggregate(EyeSample initial)
            : this()
        {
            Add(initial);
        }


        public Point2 GazePoint2D { get; private set; } = Point2.Zero;


        public Point3 GazePoint3D { get; private set; } = Point3.Zero;


        public Point3 EyePosition3D { get; private set; } = Point3.Zero;


        public Point3 EyePosition3DRelative { get; private set; } = Point3.Zero;
        

        public double PupilDiameter { get; private set; } = 0d;


        public EyeSampleAggregate Add(EyeSample eye)
        {
            GazePoint2D += eye.GazePoint2D;
            GazePoint3D += eye.GazePoint3D;
            EyePosition3D += eye.EyePosition3D;
            EyePosition3DRelative += eye.EyePosition3DRelative;
            PupilDiameter += eye.PupilDiameter;

            return this;
        }


        public EyeSampleAggregate Subtract(EyeSample eye)
        {
            GazePoint2D -= eye.GazePoint2D;
            GazePoint3D -= eye.GazePoint3D;
            EyePosition3D -= eye.EyePosition3D;
            EyePosition3DRelative -= eye.EyePosition3DRelative;
            PupilDiameter -= eye.PupilDiameter;

            return this;
        }


        public EyeSampleAggregate Normalize(int count)
        {
            GazePoint2D /= count;
            GazePoint3D /= count;
            EyePosition3D /= count;
            EyePosition3DRelative /= count;
            PupilDiameter /= count;

            return this;
        }


        public static implicit operator EyeSampleAggregate(EyeSample value)
        {
            return new EyeSampleAggregate(value);
        }
    }
}
