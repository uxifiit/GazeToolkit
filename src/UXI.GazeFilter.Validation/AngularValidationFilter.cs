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
using UXI.Serialization;
using UXI.Filters;

namespace UXI.GazeFilter.Validation
{
    public class AngularValidationFilter : Filter<ValidationPointData, ValidationResult, AngularOptions>
    {
        private readonly List<DisplayAreaChangedEvent> _displayAreaEvents = new List<DisplayAreaChangedEvent>();


        private static string ResolveDisplayAreaFilepath(string eyeTrackerDataFilePath)
        {
            string directory = Path.GetDirectoryName(eyeTrackerDataFilePath);
            string eyeTrackerDataFilename = Path.GetFileName(eyeTrackerDataFilePath);

            string eyeTrackerDisplayAreaFilename = eyeTrackerDataFilename.Replace("ET_data", "ET_display");

            return Path.Combine(directory, eyeTrackerDisplayAreaFilename);
        }


        protected override void Initialize(AngularOptions options, FilterContext context)
        {
            string displayAreaFile = String.IsNullOrWhiteSpace(options.DisplayAreaFile)
                                   ? ResolveDisplayAreaFilepath(options.InputFilePath)
                                   : options.DisplayAreaFile;

            if (File.Exists(displayAreaFile) == false)
            {
                Console.Error.WriteLine($"Eye tracker display area file not found. Searched locations:");
                if (String.IsNullOrWhiteSpace(options.DisplayAreaFile) == false)
                {
                    Console.Error.WriteLine($"- {options.DisplayAreaFile}");
                }
                if (displayAreaFile != options.DisplayAreaFile)
                {
                    Console.Error.WriteLine($"- {displayAreaFile}");
                }
            }
            else
            {
                var events = context.IO
                                    .ReadInput(displayAreaFile, FileFormat.JSON, typeof(DisplayAreaChangedEvent), null)
                                    .OfType<DisplayAreaChangedEvent>();

                foreach (var ev in events)
                {
                    _displayAreaEvents.Add(ev);
                }
            }
        }


        protected override IObservable<ValidationResult> Process(IObservable<ValidationPointData> data, AngularOptions options)
        {
            var defaultDisplayArea = _displayAreaEvents.Select(e => e.DisplayArea).FirstOrDefault();

            if (defaultDisplayArea == null)
            {
                return Observable.Empty<ValidationResult>();
            }

            return data.Buffer((a, b) => a.Point.Validation != b.Point.Validation)
                        .Select(validation =>
                        {
                            var startTime = validation.Min(v => v.Point.StartTime);
                            var displayArea = _displayAreaEvents.Where(e => e.Timestamp < startTime)
                                                                .OrderBy(e => e.Timestamp)
                                                                .Select(e => e.DisplayArea)
                                                                .LastOrDefault() ?? defaultDisplayArea;

                            var strategy = new AngularValidationEvaluationStrategy(displayArea);

                            return ValidationEvaluation.Evaluate(validation, strategy);
                        });
        }
    }
}
