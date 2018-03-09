//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reactive.Linq;
//using System.Reactive.Subjects;
//using System.Text;
//using System.Threading.Tasks;
//using UXI.GazeRx.Selection;
//using UXI.GazeRx.Smoothing;

//namespace UXI.GazeRx.Fixations.VelocityThreshold
//{

//    public static class VelocityThresholdFilterEx
//    {
//        public static IObservable<EyeMovement> FilterFixations(this IObservable<GazeData> gazeData, IVelocityThresholdFilterOptions options)
//        {
//            if (options.RequiresFillInGaps)
//            {
//                gazeData = gazeData.FillInGaps(options.FillInGaps);
//            }

//            var singleEyeGazeData = gazeData.SelectEye(options.EyeSelection);

//            if (options.RequiresNoiseReduction)
//            {
//                singleEyeGazeData = singleEyeGazeData.ReduceNoise(options.NoiseReduction);
//            }

//            if (options.VelocityCalculation.DataFrequency.HasValue == false)
//            {
//                // TODO
//            }

//            var velocities = singleEyeGazeData.CalculateVelocities(options.VelocityCalculation);

//            var movements = velocities.ClassifyByVelocity(options.VelocityThresholdClassification);

//            if (options.RequiresFixationsMerging)
//            {
//                movements = movements.MergeAdjacentFixations(options.FixationsMerging);
//            }

//            if (options.RequiresFixationsDiscarding)
//            {
//                movements = movements.DiscardShortFixations(options.FixationsDiscarding);
//            }

//            var fixations = movements.Where(m => m.MovementType == EyeMovementType.Fixation);

//            return fixations;
//        }
//    }

//    public class VelocityThresholdFilter
//    {
//        private readonly ReplaySubject<IObservable<GazeData>> _dataSources = new ReplaySubject<IObservable<GazeData>>(1);

//        public VelocityThresholdFilter()
//        {
//            _dataSources.OnNext(Observable.Empty<GazeData>());
//        }

//        public VelocityThresholdFilter(IObservable<GazeData> gazeData)
//        {

//        }

//        public VelocityThresholdFilter(IVelocityThresholdFilterOptions options)
//        {

//        }

//        public VelocityThresholdFilter(IObservable<GazeData> gazeData, IVelocityThresholdFilterOptions options)
//        {

//        }


//        public IObservable<GazeData> Raw => _dataSources.Switch();

//        public void SetSource(IObservable<GazeData> gazeData)
//        {
//            _dataSources.OnNext(gazeData);
//        }


//        public IObservable<GazeData> FilledInGaps { get; }



//        public IObservable<SingleEyeGazeData> SingleEye { get; }

//        public IObservable<int> Frequency { get; }

//        public IObservable<EyeVelocity> Velocities { get; }

//        public IObservable<EyeMovement> Movements { get; }

//        public IObservable<EyeMovement> Fixations { get; }
//    }
//}
