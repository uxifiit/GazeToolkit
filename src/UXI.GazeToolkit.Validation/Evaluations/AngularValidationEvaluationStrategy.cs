using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Utils;

namespace UXI.GazeToolkit.Validation.Evaluations
{
    public class AngularValidationEvaluationStrategy : IValidationEvaluationStrategy
    {
        public AngularValidationEvaluationStrategy()
            : this(new DisplayArea())
        { }


        public AngularValidationEvaluationStrategy(DisplayArea displayArea)
        {
            DisplayArea = displayArea;
        }


        public DisplayArea DisplayArea { get; set; }


        private Point3 ConvertTo3D(Point2 targetPoint2D)
        {
            var dx = (DisplayArea.TopRight - DisplayArea.TopLeft) * targetPoint2D.X;
            var dy = (DisplayArea.BottomLeft - DisplayArea.TopLeft) * targetPoint2D.Y;
            Point3 targetPoint = DisplayArea.TopLeft + (dx + dy);

            return targetPoint;
        }


        public ValidationPointResult Evaluate(ValidationPointData point)
        {
            var validData = point.Data
                                 .Where(d => d.LeftEye.Validity == EyeValidity.Valid && d.RightEye.Validity == EyeValidity.Valid)
                                 .ToList();

            var validLeftEyeData = validData.Select(d => d.LeftEye).ToList();
            var validRightEyeData = validData.Select(d => d.RightEye).ToList();

            return new ValidationPointResult
            (
                point: point.Point,

                leftEye: validLeftEyeData.Count() > 1
                         ? new EyeValidationPointResult()
                         {
                             Accuracy = EvaluateAccuracy(validLeftEyeData, point.Point.Position),
                             PrecisionSD = EvaluatePrecisionSD(validLeftEyeData),
                             PrecisionRMS = EvaluatePrecisionRMS(validLeftEyeData),
                             ValidRatio = (double)validLeftEyeData.Count / point.Data.Count(),
                             Distance = CalculateDistance(validLeftEyeData)
                         }
                         : new EyeValidationPointResult() { ValidRatio = 0 },

                rightEye: validRightEyeData.Count() > 1
                          ? new EyeValidationPointResult()
                          {
                              Accuracy = EvaluateAccuracy(validRightEyeData, point.Point.Position),
                              PrecisionSD = EvaluatePrecisionSD(validRightEyeData),
                              PrecisionRMS = EvaluatePrecisionRMS(validRightEyeData),
                              ValidRatio = (double)validRightEyeData.Count / point.Data.Count(),
                              Distance = CalculateDistance(validRightEyeData)
                          }
                          : new EyeValidationPointResult() { ValidRatio = 0 }
            );
        }


        private double? EvaluateAccuracy(List<EyeData> data, Point2 targetPoint)
        {
            Point3 targetPoint3D = ConvertTo3D(targetPoint);

            if (data.Any())
            {
                var averageAngleRad = data.Average(eye =>
                {
                    var directionGaze = PointUtils.Vectors.Normalize(PointUtils.Vectors.GetVector(eye.EyePosition3D, eye.GazePoint3D));
                    var directionTarget = PointUtils.Vectors.Normalize(PointUtils.Vectors.GetVector(eye.EyePosition3D, targetPoint3D));
                    return PointUtils.Vectors.GetAngle(directionGaze, directionTarget);
                });

                return MathUtils.ConvertRadToDeg(averageAngleRad);
            }

            return null;
        }


        private double EvaluatePrecisionSD(List<EyeData> eyeData)
        {
            Point3 averageGazePoint = PointUtils.Average(eyeData.Select(d => d.GazePoint3D));

            double variance = eyeData.Select(d => PointUtils.Vectors.GetAngle(
                                  PointUtils.Vectors.Normalize(PointUtils.Vectors.GetVector(d.EyePosition3D, d.GazePoint3D)),
                                  PointUtils.Vectors.Normalize(PointUtils.Vectors.GetVector(d.EyePosition3D, averageGazePoint))
                              )).Select(angleRad => MathUtils.ConvertRadToDeg(angleRad))
                                .Select(angle => Math.Pow(angle, 2))
                                .Average();

            double sd = Math.Sqrt(variance);

            return sd;
        }


        private double EvaluatePrecisionRMS(List<EyeData> eyeData)
        {
            var rms = Math.Sqrt(
                Enumerable.Zip(
                    eyeData.Take(eyeData.Count - 1),
                    eyeData.Skip(1),
                    (first, second) => PointUtils.Vectors.GetAngle(
                        PointUtils.Vectors.Normalize(PointUtils.Vectors.GetVector(first.EyePosition3D, first.GazePoint3D)),
                        PointUtils.Vectors.Normalize(PointUtils.Vectors.GetVector(second.EyePosition3D, second.GazePoint3D))
                    )
                ).Select(angleRad => MathUtils.ConvertRadToDeg(angleRad))
                 .Select(angle => Math.Pow(angle, 2))
                 .Average()
            );

            return rms;
        }


        private double? CalculateDistance(List<EyeData> eyeData)
        {
            if (eyeData.Any())
            {
                return eyeData.Average(eye => PointUtils.Vectors.GetLength(PointUtils.Vectors.GetVector(eye.EyePosition3D, eye.GazePoint3D)));
            }

            return null;
        }
    }
}
