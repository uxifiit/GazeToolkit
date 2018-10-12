using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.GazeFilter;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Fixations;
using UXI.GazeToolkit.Fixations.VelocityThreshold;
using UXI.GazeToolkit.Interpolation;
using UXI.GazeToolkit.Selection;
using UXI.GazeToolkit.Smoothing;

namespace UXI.GazeFilter.FillInGaps
{
    public class FixationFilterOptions : BaseOptions, IFillInGapsOptions, IEyeSelectionOptions, IExponentialSmoothingOptions, IVelocityCalculationOptions, IVelocityThresholdClassificationOptions, IFixationsMergingOptions, IFixationsDiscardingOptions
    {
        [Option("fillin", Default = false, HelpText = "Interpolate gaps in data.", Required = false)]
        public bool IsFillInGapsEnabled { get; set; }

        // IFillInGapsOptions
        [Option("fillin-max-gap", Default = 75, HelpText = "Interpolate data in case of missing or invalid data, if the gap in data is less or equal the max gap length.", Required = false)]
        public double MaxGapLength
        {
            get
            {
                return MaxGap.TotalMilliseconds;
            }
            set
            {
                MaxGap = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan MaxGap { get; private set; }


        // IEyeSelectionOptions
        [Option('s', "select", Default = EyeSelectionStrategy.Average, HelpText = "Which eye should be selected for the classification.", Required = true)]
        public EyeSelectionStrategy EyeSelectionStrategy { get; set; }



        [Option("denoise", Default = false, HelpText = "Reduce noise in data.", Required = false)]
        public bool IsNoiseReductionEnabled { get; set; }
        
        // IExponentialSmoothingOptions
        public NoiseReductionStrategy NoiseReductionStrategy => NoiseReductionStrategy.Exponential;

        [Option('a', "denoise-alpha", Default = ExponentialSmoothingFilter.DEFAULT_ALPHA, HelpText = "Alpha parameter for Exponential fmoothing filter", Required = false)]
        public double Alpha { get; set; }


        // IVelocityCalculationOptions
        [Option('f', "frequency", Default = null, SetName = "frequency", HelpText = "Frequency of the eye tracking data.")]
        public int? DataFrequency { get; set; }


        [Option('w', "window-side", Default = VelocityCalculationRx.DefaultTimeWindowSideMilliseconds, SetName = "frequency", HelpText = "Time window length in milliseconds to use for measuring frequency of the eye tracking data")]
        public double TimeWindowSideLength
        {
            get
            {
                return TimeWindowSide.TotalMilliseconds;
            }
            set
            {
                TimeWindowSide = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan TimeWindowSide { get; private set; }


        // IVelocityThresholdClassificationOptions
        [Option('t', "threshold", Default = 30, HelpText = "Fixation movement classification threshold.")]
        public double VelocityThreshold { get; set; }

        [Option("merge", Default = false, HelpText = "Merge short fixations.", Required = false)]
        public bool IsMergeEnabled { get; set; }

        // IFixationsMergingOptions
        [Option("merge-max-gap-between", Default = 75, HelpText = "Max time between fixations used when merging adjacent fixations.")]
        public double MaxTimeBetweenFixationsLength
        {
            get
            {
                return MaxTimeBetweenFixations.TotalMilliseconds;
            }
            set
            {
                MaxTimeBetweenFixations = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan MaxTimeBetweenFixations { get; private set; }


        [Option("merge-max-angle-between", Default = 0.5, HelpText = "Max angle between fixations used when merging adjacent fixations.")]
        public double MaxAngleBetweenFixations { get; set; }


        [Option("discard", Default = false, HelpText = "Merge short fixations.", Required = false)]
        public bool IsDiscardEnabled { get; set; }

        // IFixationsDiscardingOptions
        [Option('d', "discard-min-duration", Default = 60, HelpText = "Minimum fixation duration.")]
        public double MinimumFixationDurationLength
        {
            get
            {
                return MinimumFixationDuration.TotalMilliseconds;
            }
            set
            {
                MinimumFixationDuration = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan MinimumFixationDuration { get; private set; }
    }


    static class Program
    {
        static void Main(string[] args)
        {
            new FilterTool<GazeData, EyeMovement>
            (   
                new Filter<GazeData, EyeMovement, FixationFilterOptions>("I-VT Fixation Filtering", (source, o) => {
                    var data = o.IsFillInGapsEnabled ? source.FillInGaps(o) : source;
                    var eye = data.SelectEye(o);
                    eye = o.IsNoiseReductionEnabled ? eye.ReduceNoise(o) : eye;
                    var movements = eye.CalculateVelocities(o).ClassifyByVelocity(o);
                    movements = o.IsMergeEnabled ? movements.MergeAdjacentFixations(o) : movements;
                    movements = o.IsDiscardEnabled ? movements.DiscardShortFixations(o) : movements;
                    return movements;
                })
            ).Execute(args);
        }
    }
}
