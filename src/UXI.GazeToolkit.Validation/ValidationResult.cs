using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Validation
{
    public class ValidationResult
    {
        public ValidationResult(int id, IEnumerable<ValidationPointResult> points)
        {
            Id = id;
            Points = points.ToList();
        }

        public int Id { get; }

        public List<ValidationPointResult> Points { get; }
    }
}
