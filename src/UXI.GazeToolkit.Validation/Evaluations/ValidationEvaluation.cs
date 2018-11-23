using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Validation;

namespace UXI.GazeToolkit.Validation.Evaluations
{
    public static class ValidationEvaluation
    {
        public static ValidationResult Evaluate(int validation, IEnumerable<ValidationPointGaze> validationPoints, IValidationEvaluationStrategy strategy)
        {
            var points = validationPoints.Select(p => strategy.Evaluate(p));

            return new ValidationResult(validation, points);
        }
    }
}
