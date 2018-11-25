using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Serialization;

namespace UXI.GazeFilter
{
    public static class DataIOEx
    {
        public static IObservable<object> ReadInput(this DataIO io, BaseOptions options, Type dataType, SerializationConfiguration configuration)
        {
            return io.ReadInput(options.InputFile, options.InputFileFormat, dataType, configuration);
        }


        public static IObservable<object> WriteOutput(this IObservable<object> data, DataIO io, BaseOptions options, Type dataType, SerializationConfiguration configuration)
        {
            return io.WriteOutput(data, options.OutputFile, options.OutputFileFormat == FileFormat.Default ? options.InputFileFormat : options.OutputFileFormat, dataType, configuration);
        }


        public static void WriteOutput(this DataIO io, IEnumerable<object> data, BaseOptions options, FileFormat defaultFormat, Type dataType, SerializationConfiguration configuration)
        {
            var format = options.LogFileFormat == FileFormat.Default ? defaultFormat : options.LogFileFormat;

            io.WriteOutput(data, options.LogFile, format, dataType, configuration);
        }
    }
}
