using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeFilter.Serialization;
using UXI.GazeFilter.Serialization.Extensions;
using UXI.GazeFilter.Serialization.Json;
using UXI.GazeFilter.Serialization.Json.Converters;
using UXI.GazeToolkit;

namespace UXI.GazeFilter
{
    public class FilterIO
    {
        private readonly Dictionary<FileFormat, IDataSerializationFactory> _formats;
        public FilterIO(IEnumerable<IDataSerializationFactory> factories)
        {
            _formats = factories.ToDictionary(f => f.Format);
        }


        public IObservable<object> ReadInput(string filePath, FileFormat fileFormat, Type dataType, FilterConfiguration configuration)
        {
            // same as nested using statements, but managed by the lifetime of observable
            //
            // using (TextReader reader = CreateInputReader(filePath)) 
            // using (IDataReader dataReader = GetInputDataReader(...)) 
            // { ...
            return Observable.Using(() => CreateInputReader(filePath), (reader) =>
            {
                return Observable.Using(() => GetInputDataReader(reader, fileFormat, dataType, configuration), (dataReader) =>
                {
                    return Observable.Create<object>(observer =>
                    {
                        try
                        {
                            object data;
                            while (dataReader.TryRead(out data))
                            {
                                observer.OnNext(data);
                            }

                            observer.OnCompleted();
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                        }

                        return Disposable.Empty;
                    });
                });
            });
        }


        private IDataReader GetInputDataReader(TextReader reader, FileFormat fileType, Type dataType, FilterConfiguration configuration)
        {
            IDataSerializationFactory factory;

            if (_formats.TryGetValue(fileType, out factory))
            {
                return factory.CreateReaderForType(reader, dataType, configuration);
            }

            throw new ArgumentOutOfRangeException(nameof(fileType));
        }


        private static TextReader CreateInputReader(string targetPath)
        {
            TextReader reader;
            if (String.IsNullOrWhiteSpace(targetPath))
            {
                reader = Console.In;
            }
            else
            {
                var fileStream = new FileStream(targetPath, FileMode.Open, FileAccess.Read);
                reader = new StreamReader(fileStream);
            }

            return reader;
        }


        public IObservable<object> WriteOutput(IObservable<object> data, string filePath, FileFormat fileFormat, Type dataType, FilterConfiguration configuration)
        {
            // same as nested using statements, but managed by the lifetime of observable
            //
            // using (TextWriter writer = CreateOutputWriter(filePath)) 
            // using (IDataWriter dataWriter = GetOutputDataWriter(...)) 
            // { ...
            return Observable.Using(() => CreateOutputWriter(filePath), (TextWriter writer) =>
            {
                return Observable.Using(() => GetOutputDataWriter(writer, fileFormat, dataType, configuration), (IDataWriter dataWriter) =>
                {
                    return data.Finally(dataWriter.Close)
                               .Do(d => dataWriter.Write(d));
                });
            });
        }


        private IDataWriter GetOutputDataWriter(TextWriter writer, FileFormat fileType, Type dataType, FilterConfiguration configuration)
        {
            IDataSerializationFactory factory;

            if (_formats.TryGetValue(fileType, out factory))
            {
                return factory.CreateWriterForType(writer, dataType, configuration);
            }

            throw new ArgumentOutOfRangeException(nameof(fileType));
        }


        private static TextWriter CreateOutputWriter(string targetPath)
        {
            TextWriter outputWriter;
            if (String.IsNullOrWhiteSpace(targetPath))
            {
                outputWriter = Console.Out;
            }
            else
            {
                var fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write);
                outputWriter = new StreamWriter(fileStream);
            }

            return outputWriter;
        }
    }



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
