using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public class EyeSample
    {
        public static readonly EyeSample Empty = new EyeSample(Point2.Zero, Point3.Zero, Point3.Zero, 0d);


        public EyeSample
        (
            Point2 gazePoint2D, 
            Point3 gazePoint3D, 
            Point3 eyePosition3D, 
            double pupilDiameter
        )
        {
            GazePoint2D = gazePoint2D;
            GazePoint3D = gazePoint3D;
            EyePosition3D = eyePosition3D;
            PupilDiameter = pupilDiameter;
        }


        public EyeSample(EyeSample other)
            : this(other.GazePoint2D, other.GazePoint3D, other.EyePosition3D, other.PupilDiameter)
        {
        }


        public Point2 GazePoint2D { get; }


        public Point3 GazePoint3D { get; } 


        public Point3 EyePosition3D { get; } 

        
        public double PupilDiameter { get; } 
    }
}
