using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeFilter;
using UXI.GazeFilter.Exceptions;
using UXI.GazeFilter.Fakes;

namespace UXI.GazeFilter
{
   

    [TestClass]
    public class FilterPipelineTests
    {

        [TestMethod]
        [ExpectedException(typeof(FilterTypeMismatchException))]
        public void Initialize_FirstFilterInputTypeDoesNotMatchPipelineInput_ExceptionThrown()
        {
            var pipeline = new FilterPipeline<A, B, object>
            (
                new NullFilter<B, A, object>(),
                new NullFilter<A, B, object>()
            );

            pipeline.Initialize(null, null);
        }


        [TestMethod]
        [ExpectedException(typeof(FilterTypeMismatchException))]
        public void Initialize_LastFilterOutputTypeDoesNotMatchPipelineOutput_ExceptionThrown()
        {
            var pipeline = new FilterPipeline<A, B, object>
            (
                new NullFilter<A, B, object>(),
                new NullFilter<B, A, object>()
            );

            pipeline.Initialize(null, null);
        }


        [TestMethod]
        [ExpectedException(typeof(FilterTypeMismatchException))]
        public void Initialize_InputOutputTypeDoesNotMatchBetweenFilters_ExceptionThrown()
        {
            var pipeline = new FilterPipeline<A, B, object>
            (
                new NullFilter<A, B, object>(),
                new NullFilter<A, B, object>()
            );

            pipeline.Initialize(null, null);
        }


        [TestMethod]
        public void Initialize_FilterInputIsSubclassOfOutputTypeOfPreviousFilter()
        {
            var pipeline = new FilterPipeline<A, B, object>
            (
                new NullFilter<A, SubB, object>(),
                new NullFilter<B, B, object>()
            );

            pipeline.Initialize(null, null);
        }
    }
}
