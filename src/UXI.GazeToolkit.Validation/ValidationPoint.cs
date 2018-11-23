using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Validation
{
    public class ValidationPoint
    {
        public ValidationPoint(int validation, Point2 point, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            Validation = validation;
            Point = point;
            StartTime = startTime;
            EndTime = endTime;
        }

        public int Validation { get; }

        public Point2 Point { get; }

        public DateTimeOffset StartTime { get; }

        public DateTimeOffset EndTime { get; }
    }
}
