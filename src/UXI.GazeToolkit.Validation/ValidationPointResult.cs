using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Validation
{
    public class ValidationPointResult
    {
        internal ValidationPointResult(ValidationPoint point, EyeValidationPointResult leftEye, EyeValidationPointResult rightEye)
        {
            Point = point;
            //Point = point;
            //Position = position;
            LeftEye = leftEye;
            RightEye = rightEye;
        }

        public ValidationPoint Point { get; }

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

        public double? PupilDiameter { get; internal set; }
    }
}
