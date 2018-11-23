using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Validation
{
    public class ValidationPointGaze
    {
        public ValidationPointGaze(ValidationPoint point, IEnumerable<GazeData> data)
        {
            Point = point;
            Gaze = data?.ToList() ?? Enumerable.Empty<GazeData>();
        }

        public ValidationPoint Point { get; }

        public IEnumerable<GazeData> Gaze { get; }
    }
}
