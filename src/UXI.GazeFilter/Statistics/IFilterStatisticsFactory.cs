using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter.Statistics
{
    public interface IFilterStatisticsFactory
    {
        bool CanCreate(Type type);
        IFilterStatistics Create(IFilter filter, object options);
    }
}
