using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter
{
    public static class FilterIOEx
    {
        public static IObservable<object> ReadInput(this FilterIO io, BaseOptions options, Type dataType, FilterConfiguration configuration)
        {
            return io.ReadInput(options.InputFile, options.InputFileFormat, dataType, configuration);
        }


        public static IObservable<object> WriteOutput(this IObservable<object> data, FilterIO io, BaseOptions options, Type dataType, FilterConfiguration configuration)
        {
            return io.WriteOutput(data, options.OutputFile, options.OutputFileFormat ?? options.InputFileFormat, dataType, configuration);
        }
    }
}
