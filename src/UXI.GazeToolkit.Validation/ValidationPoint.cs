using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Validation
{
    public class ValidationPoint
    {
        public ValidationPoint(int validation, int point, Point2 position, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            Validation = validation;
            Point = point;
            Position = position;
            StartTime = startTime;
            EndTime = endTime;
        }

        public int Validation { get; }

        public int Point { get;  }

        public Point2 Position { get; }

        public DateTimeOffset StartTime { get; }

        public DateTimeOffset EndTime { get; }
    }
}
