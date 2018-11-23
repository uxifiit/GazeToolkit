using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Validation
{
    public class DisplayAreaChangedEvent
    {
        public DisplayAreaChangedEvent(DisplayArea displayArea, DateTimeOffset timestamp)
        {
            DisplayArea = displayArea;
            Timestamp = timestamp;
        }

        public DisplayArea DisplayArea { get; }
        public DateTimeOffset Timestamp { get; }
    }


    public class DisplayArea
    {
        public DisplayArea() { }

        public DisplayArea(Point3 bottomLeft, Point3 topLeft, Point3 topRight)
        {
            BottomLeft = bottomLeft;
            TopLeft = topLeft;
            TopRight = topRight;
        }

        public Point3 BottomLeft { get; }
        public Point3 TopLeft { get; }
        public Point3 TopRight { get; }
    }
}
