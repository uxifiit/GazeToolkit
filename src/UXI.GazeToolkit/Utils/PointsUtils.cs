using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Utils
{
    public static partial class PointsUtils
    {
        public static Point2 Average(Point2 pointA, Point2 pointB)
        {
            var x = (pointA.X + pointB.X) / 2;
            var y = (pointA.Y + pointB.Y) / 2;

            return new Point2(x, y);
        }


        public static Point3 Average(Point3 pointA, Point3 pointB)
        {
            var x = (pointA.X + pointB.X) / 2;
            var y = (pointA.Y + pointB.Y) / 2;
            var z = (pointA.Z + pointB.Z) / 2;

            return new Point3(x, y, z);
        }


        public static double EuclidianDistance(Point2 pointA, Point2 pointB)
        {
            return Math.Sqrt
            (
                Math.Pow(pointB.X - pointA.X, 2)
              + Math.Pow(pointB.Y - pointA.Y, 2)
            );
        }


        public static double EuclidianDistance(Point3 pointA, Point3 pointB)
        {
            return Math.Sqrt
            (
                Math.Pow(pointB.X - pointA.X, 2)
              + Math.Pow(pointB.Y - pointA.Y, 2)
              + Math.Pow(pointB.Z - pointA.Z, 2)
            );
        }


        public static Point2 Average(IEnumerable<Point2> points)
        {
            var reference = points.First();
            var aggregate = Point2.Zero;

            var rest = points.Skip(1);
            int count = 1;

            foreach (var point in rest)
            {
                aggregate += point - reference;
                count++;
            }

            Point2 average = reference + (aggregate / count);

            return average;
        }


        public static Point3 Average(IEnumerable<Point3> points)
        {
            var reference = points.First();
            var aggregate = Point3.Zero;

            var rest = points.Skip(1);
            int count = 1;

            foreach (var point in rest)
            {
                aggregate += point - reference;
                count++;
            }

            Point3 average = reference + (aggregate / count);

            return average;
        }
    }
}
