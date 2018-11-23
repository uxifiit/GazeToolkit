using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Validation.Evaluations
{
    public interface IValidationEvaluationStrategy
    {
        ValidationPointResult Evaluate(ValidationPointGaze validationPoint);
    }
}
