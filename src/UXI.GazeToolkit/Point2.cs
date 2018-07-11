using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    /// <summary>
    /// Class representing a point in 2D space.
    /// </summary>
    public struct Point2
    {
        public static readonly Point2 Zero = new Point2();

        /// <summary>
        /// Gets the X coordinate of the point.
        /// </summary>
        public double X { get; }


        /// <summary>
        /// Gets the Y coordinate of the point.
        /// </summary>
        public double Y { get; }


        /// <summary>
        /// Creates an instance of the Point2 with the given coordinates.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public Point2(double x, double y)
        {
            X = x;
            Y = y;
        }


        /// <summary>
        /// Returns the point coordinatesin in the "X;Y" format.
        /// </summary>
        /// <returns>String representation of the point coordinates split by a semicolon</returns>
        public override string ToString()
        {
            return $"{X};{Y}";
        }


        public string ToString(IFormatProvider provider)
        {
            return ToString(";", provider);
        }


        public string ToString(string delimiter, IFormatProvider provider)
        {
            return $"{X.ToString(provider)}{delimiter}{Y.ToString(provider)}";
        }


        public static bool operator ==(Point2 a, Point2 b)
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
                && a.Y == b.Y;
        }


        public static bool operator !=(Point2 a, Point2 b)
        {
            return !(a == b);
        }


        public static Point2 operator +(Point2 point, double constant)
        {
            return new Point2(point.X + constant, point.Y + constant);
        }


        public static Point2 operator +(Point2 point, Point2 second)
        {
            return new Point2(point.X + second.X, point.Y + second.Y);
        }


        public static Point2 operator -(Point2 point, double constant)
        {
            return new Point2(point.X - constant, point.Y - constant);
        }


        public static Point2 operator -(Point2 point, Point2 second)
        {
            return new Point2(point.X - second.X, point.Y - second.Y);
        }


        public static Point2 operator *(Point2 point, double constant)
        {
            return new Point2(point.X * constant, point.Y * constant);
        }


        public static Point2 operator /(Point2 point, double constant)
        {
            return new Point2(point.X / constant, point.Y / constant);
        }


        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            if ((obj is Point2) == false)
            {
                return false;
            }

            var point = (Point2)obj;

            // Return true if the fields match:
            return X == point.X
                && Y == point.Y;
        }


        public bool Equals(Point2 point)
        {
            // If parameter is null return false:
            if ((object)point == null)
            {
                return false;
            }

            // Return true if the fields match:
            return X == point.X
                && Y == point.Y;
        }


        public override int GetHashCode()
        {                 
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
