using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Validation
{
    public class ValidationPointData
    {
        public ValidationPointData(ValidationPoint point, IEnumerable<GazeData> data)
        {
            Point = point;
            Data = data?.ToList() ?? Enumerable.Empty<GazeData>();
        }

        public ValidationPoint Point { get; }

        public IEnumerable<GazeData> Data { get; }
    }
}
