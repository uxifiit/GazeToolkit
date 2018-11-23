using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Serialization;
using UXI.GazeToolkit.Validation;
using UXI.GazeToolkit.Extensions;
using UXI.GazeToolkit.Validation.Evaluations;

namespace UXI.GazeFilter.Validation
{
    public class PixelValidationFilter : Filter<ValidationPointGaze, ValidationResult, PixelOptions>
    {
        private static Point2 ReadScreenResolution(PixelOptions options)
        {
            double width = options.ScreenResolutionWidth;
            double height = options.ScreenResolutionHeight;

            if (String.IsNullOrWhiteSpace(options.ScreenResolution) == false)
            {
                string[] resolution = options.ScreenResolution.Split(new[] { 'x', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (resolution.Length == 2)
                {
                    width = Convert.ToDouble(resolution[0]);
                    height = Convert.ToDouble(resolution[1]);
                }
            }

            return new Point2(width, height);
        }


        protected override void Initialize(PixelOptions options, FilterContext context) { }


        protected override IObservable<ValidationResult> Process(IObservable<ValidationPointGaze> data, PixelOptions options)
        {
            Point2 resolution = ReadScreenResolution(options);

            var strategy = new PixelValidationEvaluationStrategy(resolution.X, resolution.Y);

            return data.Buffer((a, b) => a.Point.Validation != b.Point.Validation)
                       .Select(validation =>
                       {
                           return ValidationEvaluation.Evaluate(validation.First().Point.Validation, validation, strategy);
                       });
        }
    }
}