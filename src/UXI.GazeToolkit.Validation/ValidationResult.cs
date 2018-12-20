using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Validation
{
    public class ValidationResult
    {
        public ValidationResult(IEnumerable<ValidationPointResult> points)
        {
            Points = points.ToList();
        }

        public List<ValidationPointResult> Points { get; }
    }
}
