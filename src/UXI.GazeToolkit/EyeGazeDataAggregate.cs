using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit
{
    internal class EyeGazeDataAggregate
    {
        public EyeGazeDataAggregate() { }
        public EyeGazeDataAggregate(EyeGazeData initial)
            : this()
        {
            Add(initial);
        }


        public Point2 GazePoint2D { get; private set; } = Point2.Default;

        public Point3 GazePoint3D { get; private set; } = Point3.Default;

        public Point3 EyePosition3D { get; private set; } = Point3.Default;

        public Point3 EyePosition3DRelative { get; private set; } = Point3.Default;
        
        public double PupilDiameter { get; private set; } = 0d;


        public EyeGazeDataAggregate Add(EyeGazeData eye)
        {
            GazePoint2D += eye.GazePoint2D;
            GazePoint3D += eye.GazePoint3D;
            EyePosition3D += eye.EyePosition3D;
            EyePosition3DRelative += eye.EyePosition3DRelative;
            PupilDiameter += eye.PupilDiameter;

            return this;
        }


        public EyeGazeDataAggregate Subtract(EyeGazeData eye)
        {
            GazePoint2D -= eye.GazePoint2D;
            GazePoint3D -= eye.GazePoint3D;
            EyePosition3D -= eye.EyePosition3D;
            EyePosition3DRelative -= eye.EyePosition3DRelative;
            PupilDiameter -= eye.PupilDiameter;

            return this;
        }
    }
}
