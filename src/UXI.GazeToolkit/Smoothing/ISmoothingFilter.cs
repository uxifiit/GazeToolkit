using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Smoothing
{
    public interface ISmoothingFilter<TItem>
    {
        IObservable<TItem> Smooth(IObservable<TItem> source);
    }
}
