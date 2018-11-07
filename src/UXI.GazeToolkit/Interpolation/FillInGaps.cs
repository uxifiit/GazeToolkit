using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Extensions;
using UXI.GazeToolkit.Selection;

namespace UXI.GazeToolkit.Interpolation
{
    public interface IFillInGapsOptions
    {
        TimeSpan MaxGap { get; }
    }


    public static class FillInGapsInterpolation
    {
        private static IEnumerable<EyeData> Interpolate(EyeData start, EyeData end, int interpolatedSteps)
        {
            int steps = interpolatedSteps + 1;

            Point2 stepGazePoint2D = (end.GazePoint2D - start.GazePoint2D) / steps;
            Point3 stepGazePoint3D = (end.GazePoint3D - start.GazePoint3D) / steps;
            Point3 stepEyePosition3D = (end.EyePosition3D - start.EyePosition3D) / steps;
            double stepPupilDiameter = (end.PupilDiameter - start.PupilDiameter) / steps;

            foreach (var step in Enumerable.Range(1, interpolatedSteps))
            {
                yield return new EyeData
                (
                    EyeValidity.Valid,
                    stepGazePoint2D * step + start.GazePoint2D,
                    stepGazePoint3D * step + start.GazePoint3D,
                    stepEyePosition3D * step + start.EyePosition3D,
                    stepPupilDiameter * step + start.PupilDiameter
                );
            }
        }



        //public static class PointsEx
        //{
        //    public static Point2 Interpolate(int index, Point2 start, Point2 end, int count)
        //    {
        //        var x = LinearInterpolation.Interpolate(start.X, end.X, index, count);
        //        var y = LinearInterpolation.Interpolate(start.Y, end.Y, index, count);

        //        return new Point2(x, y);
        //    }


        //    public static Point3 Interpolate(int index, Point3 start, Point3 end, int count)
        //    {
        //        var x = LinearInterpolation.Interpolate(start.X, end.X, index, count);
        //        var y = LinearInterpolation.Interpolate(start.Y, end.Y, index, count);
        //        var z = LinearInterpolation.Interpolate(start.Z, end.Z, index, count);

        //        return new Point3(x, y, z);
        //    }
        //}

        public static IObservable<EyeData> FillInGaps(this IObservable<SingleEyeGazeData> eyeGazeData, TimeSpan maxGapLength)
        {
            return Observable.Create<EyeData>(o =>
            {
                SingleEyeGazeData lastValidSample = null;
                int invalidSamplesCount = 0;

                return eyeGazeData.Subscribe(currentSample =>
                {
                    if (currentSample.Validity.HasEye())
                    {
                        if (invalidSamplesCount > 0 && lastValidSample != null)
                        {   
                            foreach (var interpolatedSample in Interpolate(lastValidSample, currentSample, invalidSamplesCount))
                            {
                                o.OnNext(interpolatedSample);
                            }

                            invalidSamplesCount = 0;
                        }

                        lastValidSample = currentSample;

                        o.OnNext(currentSample);
                    }
                    else
                    {
                        if (lastValidSample != null)
                        {
                            if (currentSample.Timestamp.Subtract(lastValidSample.Timestamp).Duration() <= maxGapLength) 
                            {
                                invalidSamplesCount++;
                            }
                            else 
                            {
                                lastValidSample = null;

                                while (invalidSamplesCount-- > 0)
                                {
                                    o.OnNext(EyeData.Default);
                                }

                                invalidSamplesCount = 0;

                                o.OnNext(currentSample);
                            }
                        }
                        else
                        {
                            o.OnNext(currentSample);
                        }
                    }
                }, o.OnError, o.OnCompleted);
            });
        }


        public static IObservable<GazeData> FillInGaps(this IObservable<GazeData> gazeData, TimeSpan maxGapLength)
        {
            // split gaze data into 2 separate observables for each eye
            var leftEye = LeftEyeSelector.Instance.SelectSingleEye(gazeData);
            var rightEye = RightEyeSelector.Instance.SelectSingleEye(gazeData);

            // fill in gaps with interpolation
            // then create EyeData instances to make sure that no other (derived) type is returned
            var leftEyeWithFilledInGaps = FillInGaps(leftEye, maxGapLength).Select(e => new EyeData(e));
            var rightEyeWithFilledInGaps = FillInGaps(rightEye, maxGapLength).Select(e => new EyeData(e));

            // combine separated samples for each eye into single sample with both eyes.
            return Observable.Zip
            (
                gazeData,
                leftEyeWithFilledInGaps,
                rightEyeWithFilledInGaps,
                (source, left, right) => new GazeData(left, right, source.Timestamp)
            );
        }


        public static IObservable<GazeData> FillInGaps(this IObservable<GazeData> gazeData, IFillInGapsOptions options)
        {
            return FillInGaps(gazeData, options.MaxGap);
        }
    }
}
