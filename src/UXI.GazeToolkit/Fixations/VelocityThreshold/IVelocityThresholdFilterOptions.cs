//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UXI.GazeRx.Selection;
//using UXI.GazeRx.Smoothing;

//namespace UXI.GazeRx.Fixations.VelocityThreshold
//{
//    public interface IVelocityThresholdFilterOptions
//    {
//        bool RequiresFillInGaps { get; }
//        IFillInGapsOptions FillInGaps { get; }


//        IEyeSelectionOptions EyeSelection { get; }


//        bool RequiresNoiseReduction { get; }
//        INoiseReductionOptions NoiseReduction { get; }


//        IVelocityCalculationOptions VelocityCalculation { get; }


//        IVelocityThresholdClassificationOptions VelocityThresholdClassification { get; }


//        bool RequiresFixationsMerging { get; }
//        IFixationsMergingOptions FixationsMerging { get; }


//        bool RequiresFixationsDiscarding { get; }
//        IFixationsDiscardingOptions FixationsDiscarding { get; }
//    }
//}
