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



    // Fills in gaps when the data is missing:
    // ....----..-.-.-.--.-.-----.-----.      
    // Method splits the stream into buffers starting with valid data until other valid data is found
    // .|.|.|.----|.|.-|.-|.--|.-
    // Then it takes every overlapping pair of 2 buffers and interpolates invalid data from the first of those two.
    // .|.|.|.....|.|..|..|...|..
    // Finally it merges first buffers from the pairs
    // .................................
    public static class FillInGapsInterpolation
    {
        private static IEnumerable<EyeGazeData> Interpolate(EyeGazeData start, EyeGazeData end, int interpolatedSteps)
        {
            int steps = interpolatedSteps + 1;

            Point2 stepGazePoint2D = (end.GazePoint2D - start.GazePoint2D) / steps;
            Point3 stepGazePoint3D = (end.GazePoint3D - start.GazePoint3D) / steps;
            Point3 stepEyePosition3D = (end.EyePosition3D - start.EyePosition3D) / steps;
            Point3 stepEyePosition3DRelative = (end.EyePosition3DRelative - start.EyePosition3DRelative) / steps;
            double stepPupilDiameter = (end.PupilDiameter - start.PupilDiameter) / steps;

            foreach (var step in Enumerable.Range(1, interpolatedSteps))
            {
                yield return new EyeGazeData
                (
                    start.Validity,
                    stepGazePoint2D * step + start.GazePoint2D,
                    stepGazePoint3D * step + start.GazePoint3D,
                    stepEyePosition3D * step + start.EyePosition3D,
                    stepEyePosition3DRelative * step + start.EyePosition3DRelative,
                    stepPupilDiameter * step + start.PupilDiameter
                );
            }
        }


        //private static IList<EyeGazeData> Interpolate(IList<IList<EyeGazeData>> ll, TimeSpan maxGapLength)
        //{
        //    if (ll.Count == 2 && ll.First().Count > 1)
        //    {
        //        var first = ll[0];
        //        var second = ll[1];

        //        var validStart = first.First();
        //        var validEnd = second.First();

        //        if (validEnd.TimeStamp - validStart.TimeStamp <= maxGapLength)
        //        {
                    

        //            return result;
        //        }
        //    }

        //    return ll.First();
        //}


        //public static IObservable<SingleEyeGazeData> Interpolate(this IObservable<SingleEyeGazeData> gaze, TimeSpan maxGapLength)
        //{
        //    // buffer gaze data into subsequences where each subsequence starts with valid sample followed by invalid samples
        //    var gaps = gaze.Buffer(eye => eye.Validity.HasEye());

        //    // continuously take overlapping tuples of subsequences with samples and fill in gaps between valid samples in tuples 
        //    var gapsFillingIn = gaps.Buffer(2, 1).Select(gap => Interpolate(gap, maxGapLength));

        //    // flatten the sequence
        //    return gapsFillingIn.SelectMany(g => g);
        //}


        public static IObservable<EyeGazeData> FillInGaps(this IObservable<SingleEyeGazeData> eyeGazeData, TimeSpan maxGapLength)
        {
            return Observable.Create<EyeGazeData>(o =>
            {
                SingleEyeGazeData lastValidSample = null;
                int invalidSamples = 0;

                return eyeGazeData.Subscribe(sample =>
                {
                    if (sample.Validity.HasEye())
                    {
                        if (invalidSamples > 0 && lastValidSample != null)
                        {   
                            foreach (var interpolatedSample in Interpolate(lastValidSample, sample, invalidSamples))
                            {
                                o.OnNext(interpolatedSample);
                            }

                            invalidSamples = 0;
                        }

                        lastValidSample = sample;

                        o.OnNext(sample);
                    }
                    else
                    {
                        if (lastValidSample != null)
                        {
                            if ((sample.Timestamp - lastValidSample.Timestamp) * 10 <= maxGapLength.Ticks) 
                            {
                                invalidSamples++;
                            }
                            else 
                            {
                                lastValidSample = null;

                                while (invalidSamples-- > 0)
                                {
                                    o.OnNext(EyeGazeData.Empty);
                                }

                                invalidSamples = 0;

                                o.OnNext(sample);
                            }
                        }
                        else
                        {
                            o.OnNext(sample);
                        }
                    }
                }, o.OnError, o.OnCompleted);
            });
        }


        public static IObservable<GazeData> FillInGaps(this IObservable<GazeData> gazeData, TimeSpan maxGapLength)
        {
            // split gaze data into 2 separate observables corresponding each eye
            var leftEye = LeftEyeSelector.Instance.SelectSingleEye(gazeData);
            var rightEye = RightEyeSelector.Instance.SelectSingleEye(gazeData);

            var leftEyeWithFilledInGaps = FillInGaps(leftEye, maxGapLength).Select(e => new EyeGazeData(e));
            var rightEyeWithFilledInGaps = FillInGaps(rightEye, maxGapLength).Select(e => new EyeGazeData(e));

            // combine separated samples for each eye into single sample with both eyes.
            return Observable.Zip
            (
                gazeData,
                leftEyeWithFilledInGaps,
                rightEyeWithFilledInGaps,
                (source, left, right) => new GazeData(left.Validity.MergeToEyeValidity(right.Validity), left, right, source.Timestamp)
            );
        }


        public static IObservable<GazeData> FillInGaps(this IObservable<GazeData> gazeData, IFillInGapsOptions options)
        {
            return FillInGaps(gazeData, options.MaxGap);
        }
    }
}
