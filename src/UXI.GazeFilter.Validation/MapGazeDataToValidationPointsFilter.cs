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
using UXI.Serialization;
using UXI.Filters;

namespace UXI.GazeFilter.Validation
{
    public class MapGazeDataToValidationPointsFilter<TValidationOptions> : Filter<GazeData, ValidationPointData, TValidationOptions>
        where TValidationOptions : ValidationOptions
    {
        private readonly List<ValidationPoint> _points = new List<ValidationPoint>();


        protected override void Initialize(TValidationOptions options, FilterContext context)
        {
            if (File.Exists(options.ValidationPointsFile) == false)
            {
                Console.Error.WriteLine($"Validation points file not found at: {options.ValidationPointsFile}");
            }
            else
            {
                var points = context.IO
                                    .ReadInput(options.ValidationPointsFile, FileFormat.CSV, typeof(ValidationPoint), null)
                                    .OfType<ValidationPoint>();

                foreach (var point in points)
                {
                    _points.Add(point);
                }

                _points.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
            }
        }


        protected override IObservable<ValidationPointData> Process(IObservable<GazeData> data, TValidationOptions options)
        {
            if (_points.Any() == false)
            {
                return Observable.Empty<ValidationPointData>();
            }

            var points = _points.ToList();

            return Observable.Create<ValidationPointData>(observer =>
            {
                int index = -1;
                var pointData = new List<GazeData>();
                DateTimeOffset startTime = DateTimeOffset.MinValue;
                DateTimeOffset endTime = DateTimeOffset.MinValue;

                return data.Subscribe(gaze =>
                {
                    int newIndex = index;

                    while (newIndex == -1
                        || (newIndex < points.Count && gaze.Timestamp > startTime && gaze.Timestamp >= endTime))
                    {
                        newIndex++;
                        if (newIndex < points.Count)
                        {
                            startTime = points[newIndex].StartTime.Add(TimeSpan.FromMilliseconds(800));
                            endTime = startTime.Add(TimeSpan.FromMilliseconds(1000));
                        }
                    }

                    if (newIndex != index)
                    {
                        if (index > -1 && index < points.Count)
                        {
                            observer.OnNext(new ValidationPointData(points[index], pointData));
                        }
                        pointData.Clear();

                        index = newIndex;
                    }

                    if (index >= points.Count)
                    {
                        observer.OnCompleted();
                    }
                    else if (index > -1 && gaze.Timestamp >= startTime && gaze.Timestamp < endTime)
                    {
                        pointData.Add(gaze);
                    }
                }, () =>
                {
                    // flush remaining points
                    if (index < 0)
                    {
                        index = 0;
                    }

                    while (index < points.Count)
                    {
                        observer.OnNext(new ValidationPointData(points[index], pointData));
                        index++;
                        pointData.Clear();
                    }

                    observer.OnCompleted();
                });
            });
        }
    }
}
