using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Validation
{
    public class ValidationPointResult
    {
        internal ValidationPointResult(Point2 targetPoint, EyeValidationPointResult leftEye, EyeValidationPointResult rightEye)
        {
            TargetPoint2D = targetPoint;
            LeftEye = leftEye;
            RightEye = rightEye;
        }

        public Point2 TargetPoint2D { get; }

        public EyeValidationPointResult LeftEye { get; }

        public EyeValidationPointResult RightEye { get; }
    }


    public class EyeValidationPointResult
    {
        public double? Accuracy { get; internal set; }

        public double ValidRatio { get; internal set; }

        public double? PrecisionSD { get; internal set; }

        public double? PrecisionRMS { get; internal set; }

        public double? Distance { get; internal set; }
    }
}
