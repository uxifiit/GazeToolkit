﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter
{
    public interface IFilter
    {
        Type InputType { get; }

        Type OutputType { get; }

        Type OptionsType { get; }

        IObservable<object> Process(IObservable<object> data, object options);
    }


    public static class IFilterEx
    {
        public static IObservable<object> Process(this IObservable<object> data, IFilter filter, object options)
        {
            return filter.Process(data, options);
        }
    }
}