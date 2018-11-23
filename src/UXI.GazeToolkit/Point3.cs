using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    /// <summary>
    /// Class representing a point in 3D space.
    /// </summary>
    public struct Point3
    {
        public readonly static Point3 Zero = new Point3();

        /// <summary>
        /// Gets the X coordinate of the point.
        /// </summary>
        public double X { get; }


        /// <summary>
        /// Gets the Y coordinate of the point.
        /// </summary>
        public double Y { get; }


        /// <summary>
        /// Gets the Z coordinate of the point.
        /// </summary>
        public double Z { get; }


        /// <summary>
        /// Creates an instance of the Point3 with the given coordinates.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        public Point3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }


        public static bool operator ==(Point3 a, Point3 b)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.X == b.X
                && a.Y == b.Y
                && a.Z == b.Z;
        }


        public static bool operator !=(Point3 a, Point3 b)
        {
            return !(a == b);
        }


        public static Point3 operator +(Point3 point, double constant)
        {
            return new Point3(point.X + constant, point.Y + constant, point.Z + constant);
        }


        public static Point3 operator +(Point3 point, Point3 second)
        {
            return new Point3(point.X + second.X, point.Y + second.Y, point.Z + second.Z);
        }


        public static Point3 operator -(Point3 point, double constant)
        {
            return new Point3(point.X - constant, point.Y - constant, point.Z - constant);
        }


        public static Point3 operator -(Point3 point, Point3 second)
        {
            return new Point3(point.X - second.X, point.Y - second.Y, point.Z - second.Z);
        }


        public static Point3 operator *(Point3 point, double constant)
        {
            return new Point3(point.X * constant, point.Y * constant, point.Z * constant);
        }


        public static Point3 operator /(Point3 point, double constant)
        {
            return new Point3(point.X / constant, point.Y / constant, point.Z / constant);
        }


        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            if ((obj is Point3) == false)
            {
                return false;
            }

            var point = (Point3)obj;

            // Return true if the fields match:
            return X == point.X
                && Y == point.Y
                && Z == point.Z;
        }


        public bool Equals(Point3 point)
        {
            // If parameter is null return false:
            if ((object)point == null)
            {
                return false;
            }

            // Return true if the fields match:
            return X == point.X
                && Y == point.Y
                && Z == point.Z;
        }


        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }
    }
}
