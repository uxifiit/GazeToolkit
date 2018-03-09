using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit.Selection
{
    public interface ISingleEyeGazeDataSelector
    {
        SingleEyeGazeData SelectSingleEye(GazeData gazeData);
    }
}
