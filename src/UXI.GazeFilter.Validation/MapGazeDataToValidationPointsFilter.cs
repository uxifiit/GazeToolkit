﻿using System;
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

            return Observable.Create<ValidationPointData>(observer =>
            {
                var points = _points.GetEnumerator();
                if (points.MoveNext())
                {
                    var currentPoint = points.Current;
                    var pointData = new List<GazeData>();
                    var startTime = currentPoint.StartTime.Add(TimeSpan.FromMilliseconds(800));
                    var endTime = startTime.Add(TimeSpan.FromMilliseconds(1000));

                    return data.Subscribe(gaze =>
                    {
                        if (gaze.Timestamp >= startTime && gaze.Timestamp < endTime)
                        {
                            pointData.Add(gaze);
                        }
                        else if (gaze.Timestamp > endTime)
                        {
                            observer.OnNext(new ValidationPointData(currentPoint, pointData));
                            pointData = new List<GazeData>();
                            currentPoint = null;

                            if (points.MoveNext())
                            {
                                currentPoint = points.Current;
                                startTime = currentPoint.StartTime.Add(TimeSpan.FromMilliseconds(800));
                                endTime = startTime.Add(TimeSpan.FromMilliseconds(1000));
                            }
                            else
                            {
                                observer.OnCompleted();
                            }
                        }
                    }, () =>
                    {
                        if (currentPoint != null)
                        {
                            observer.OnNext(new ValidationPointData(currentPoint, pointData));
                        }
                        observer.OnCompleted();
                    });
                }
                else
                {
                    return Disposable.Empty;
                }
            });
        }
    }
}
