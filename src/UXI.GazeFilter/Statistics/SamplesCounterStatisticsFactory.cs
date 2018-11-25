using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter.Statistics
{
    public class SamplesCounterStatisticsFactory : IFilterStatisticsFactory
    {
        public bool CanCreate(Type type)
        {
            return true;
        }

        public IFilterStatistics Create(IFilter filter, object options)
        {
            return new SamplesCounterStatistics(filter.GetType().Name);
        }
    }
}
