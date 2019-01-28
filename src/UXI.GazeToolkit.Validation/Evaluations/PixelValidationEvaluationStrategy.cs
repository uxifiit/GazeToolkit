using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Utils;

namespace UXI.GazeToolkit.Validation.Evaluations
{
    public class PixelValidationEvaluationStrategy : IValidationEvaluationStrategy
    {
        public PixelValidationEvaluationStrategy()
        {

        }


        public PixelValidationEvaluationStrategy(double screenWidth, double screenHeight)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = ScreenHeight;
        }


        public double ScreenWidth { get; set; } = 1;

        public double ScreenHeight { get; set; } = 1;


        private Point2 ConvertToPixels(Point2 screenPoint)
        {
            return new Point2(screenPoint.X * ScreenWidth, screenPoint.Y * ScreenHeight);
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
                             ValidRatio = (double)validLeftEyeData.Count / validData.Count,
                             Distance = CalculateDistance(validLeftEyeData),
                             PupilDiameter = CalculatePupilDiameter(validLeftEyeData)
                         }
                         : new EyeValidationPointResult() { ValidRatio = 0 },
                         
                rightEye: validRightEyeData.Count() > 1
                          ? new EyeValidationPointResult()
                          {
                              Accuracy = EvaluateAccuracy(validRightEyeData, point.Point.Position),
                              PrecisionSD = EvaluatePrecisionSD(validRightEyeData),
                              PrecisionRMS = EvaluatePrecisionRMS(validRightEyeData),
                              ValidRatio = (double)validRightEyeData.Count / validData.Count,
                              Distance = CalculateDistance(validRightEyeData),
                              PupilDiameter = CalculatePupilDiameter(validRightEyeData)
                          }
                          : new EyeValidationPointResult() { ValidRatio = 0 }
            );
        }


        private double? EvaluateAccuracy(List<EyeData> data, Point2 targetPoint)
        {
            var targetPointPx = ConvertToPixels(targetPoint);

            if (data.Any())
            {
                return data.Select(eye => ConvertToPixels(eye.GazePoint2D))
                           .Average(point => PointUtils.EuclidianDistance(point, targetPointPx));
            }

            return null;
        }

        
        private double EvaluatePrecisionSD(List<EyeData> data)
        {
            Point2 averageGazePoint = PointUtils.Average(data.Select(d => d.GazePoint2D));

            Point2 averageGazePointPx = ConvertToPixels(averageGazePoint);

            double variance = data.Select(eye => ConvertToPixels(eye.GazePoint2D))
                                  .Select(point => PointUtils.EuclidianDistance(point, averageGazePointPx))
                                  .Select(distance => Math.Pow(distance, 2))
                                  .Average();

            double sd = Math.Sqrt(variance);

            return sd;
        }


        private double EvaluatePrecisionRMS(List<EyeData> data)
        {
            var points = data.Select(eye => ConvertToPixels(eye.GazePoint2D));

            var rms = Math.Sqrt(
                Enumerable.Zip(
                    points.Take(data.Count - 1),
                    points.Skip(1),
                    (first, second) => PointUtils.EuclidianDistance(first, second)
                ).Select(distance => Math.Pow(distance, 2))
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


        private double? CalculatePupilDiameter(List<EyeData> validRightEyeData)
        {
            if (validRightEyeData.Any())
            {
                return validRightEyeData.Average(eye => eye.PupilDiameter);
            }

            return null;
        }
    }
}
