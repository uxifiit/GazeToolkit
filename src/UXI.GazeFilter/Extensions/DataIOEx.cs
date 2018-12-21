using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Serialization;
using UXI.Serialization.Reactive;
using UXI.Serialization;

namespace UXI.GazeFilter
{
    public static class DataIOEx
    {
        public static IObservable<object> ReadInput(this DataIO io, BaseOptions options, Type dataType, SerializationSettings settings)
        {
            return DataIORx.ReadInputAsObservable(io, options.InputFile, options.InputFileFormat, dataType, settings);
        }


        public static IObservable<object> WriteOutput(this IObservable<object> data, DataIO io, BaseOptions options, Type dataType, SerializationSettings settings)
        {
            return DataIORx.WriteOutput(io, data, options.OutputFile, options.OutputFileFormat == FileFormat.Default ? options.InputFileFormat : options.OutputFileFormat, dataType, settings);
        }


        public static void WriteOutput(this DataIO io, IEnumerable<object> data, BaseOptions options, FileFormat defaultFormat, Type dataType, SerializationSettings settings)
        {
            var format = options.LogFileFormat == FileFormat.Default ? defaultFormat : options.LogFileFormat;

            io.WriteOutput(data, options.LogFile, format, dataType, settings);
        }
    }
}
