using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeRx.Fixations.VelocityThreshold.Steps
{
    public class BaseOptions
    {
        public BaseOptions Clone() { return (BaseOptions)this.MemberwiseClone(); }

        public event EventHandler OptionsChanged;
        protected void OnOptionsChanged() => OptionsChanged?.Invoke(this, EventArgs.Empty);
    }


    public class FillInGapsOptions : BaseOptions, IFillInGapsOptions
    {
        private TimeSpan maxGapLength = TimeSpan.FromMilliseconds(75);
        public TimeSpan MaxGapLength { get { return maxGapLength; } set { maxGapLength = value; OnOptionsChanged(); } }
    }

    public class NoiseReductionOptions : BaseOptions, INoiseReductionOptions
    {
        private NoiseReductionStrategy strategy = NoiseReductionStrategy.Exponential;
        public NoiseReductionStrategy Strategy { get { return strategy; } set { strategy = value; OnOptionsChanged(); } }
    }

    public class EyeSelectionOptions : BaseOptions
    {
        private EyeSelectionStrategy eyeSelection = EyeSelectionStrategy.Average;
        public EyeSelectionStrategy EyeSelection { get { return eyeSelection; } set { eyeSelection = value; OnOptionsChanged(); } }
    }



    public class FixationsClassificationOptions : BaseOptions
    {
        public const int DEFAULT_FREQUENCY = 60;

        private int? frequency = DEFAULT_FREQUENCY;
        public int? Frequency { get { return frequency; } set { frequency = value; OnOptionsChanged(); } }

        private TimeSpan velocityEvaluationTimeWindow = TimeSpan.FromMilliseconds(20);
        public TimeSpan VelocityEvaluationTimeWindow { get { return velocityEvaluationTimeWindow; } set { velocityEvaluationTimeWindow = value; OnOptionsChanged(); } }

        private double classificationThreshold = 30d;
        public double ClassificationThreshold { get { return classificationThreshold; } set { classificationThreshold = value; OnOptionsChanged(); } }
    }


    public class MergeAdjacentFixationsOptions : BaseOptions
    {
        private TimeSpan maxTimeBetweenFixations = TimeSpan.FromMilliseconds(75);
        public TimeSpan MaxTimeBetweenFixations { get { return maxTimeBetweenFixations; } set { maxTimeBetweenFixations = value; OnOptionsChanged(); } }


        private double maxAngleBetweenFixations = 0.5d;
        public double MaxAngleBetweenFixations { get { return maxAngleBetweenFixations; } set { maxAngleBetweenFixations = value; OnOptionsChanged(); } }
    }


    public class DiscardShortFixationsOptions : BaseOptions
    {
        private TimeSpan minFixationDuration = TimeSpan.FromMilliseconds(60);
        public TimeSpan MinFixationDuration { get { return minFixationDuration; } set { minFixationDuration = value; OnOptionsChanged(); } }
    }
}
