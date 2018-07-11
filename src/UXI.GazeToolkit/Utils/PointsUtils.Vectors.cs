using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Utils
{
    public static partial class PointsUtils
    {
        public static class Vectors
        {
            /// <summary>
            /// Creates a vector from the starting point to the end point.
            /// </summary>
            /// <param name="startPoint"></param>
            /// <param name="endPoint"></param>
            /// <returns></returns>
            public static Point3 GetVector(Point3 startPoint, Point3 endPoint)
            {
                return endPoint - startPoint;
            }


            /// <summary>
            /// Gets the length of the vector.
            /// </summary>
            /// <param name="vector"></param>
            /// <returns></returns>
            public static double GetLength(Point3 vector)
            {
                return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2) + Math.Pow(vector.Z, 2));
            }

            
            /// <summary>
            /// Returns the vector normalized with its length.
            /// </summary>
            /// <param name="vector"></param>
            /// <returns></returns>
            public static Point3 Normalize(Point3 vector)
            {
                double length = GetLength(vector);
                return vector / length;
            }


            /// <summary>
            /// Gets the dot product of two vectors.
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            public static double DotProduct(Point3 left, Point3 right)
            {
                return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
            }


            /// <summary>
            /// Gets the angle between two vectors in radians.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="target"></param>
            /// <returns></returns>
            public static double GetAngle(Point3 source, Point3 target)
            {
                return Math.Acos
                (
                    DotProduct(source, target)
                    /
                    (GetLength(source) * GetLength(target))
                );
            }
        }
    }
}
