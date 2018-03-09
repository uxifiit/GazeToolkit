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
    public enum InputFileType
    {
        JSON,
        CSV,  // not supported yet
    }



    public enum OutputFileType
    {
        CSV, // not supported
        TSV, // not supported
        JSON
    }



    public static class FilterIO
    {
        static readonly List<IDataSerializationFactory> formats = new List<IDataSerializationFactory>()
        {
            new JsonSerializationFactory(DataJsonConverters.Converters)
        };


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


        private static IDataReader GetInputDataReader<TResult>(TextReader reader, InputFileType fileType)
        {
            IDataSerializationFactory format = null;

            switch (fileType)
            {
                case InputFileType.JSON:
                    format = formats.FirstOrDefault(f => f.FormatName == "JSON");
                    break;
            }

            if (format == null)
            {
                throw new ArgumentOutOfRangeException(nameof(fileType));
            }

            return format.CreateReaderForType(reader, typeof(TResult));
        }


        private static IDataWriter GetOutputDataWriter<TResult>(TextWriter writer, OutputFileType fileType)
        {
            IDataSerializationFactory format = null;

            switch (fileType)
            {
                case OutputFileType.JSON:
                    format = formats.FirstOrDefault(f => f.FormatName == "JSON");
                    break;
            }

            if (format == null)
            {
                throw new ArgumentOutOfRangeException(nameof(fileType));
            }

            return format.CreateWriterForType(writer, typeof(TResult));
        }


        public static IObservable<TResult> ReadInput<TResult>(BaseOptions options)
        {
            return ReadInput<TResult>(options.InputFile, options.InputFileType);
        }


        public static IObservable<TResult> ReadInput<TResult>(string inputFile, InputFileType inputFileType)
        {
            return Observable.Using(() => CreateInputReader(inputFile),
                (reader) => Observable.Using(() => GetInputDataReader<TResult>(reader, inputFileType),
                    (dataReader) => Observable.Create<TResult>(observer =>
                    {
                        try
                        {
                            foreach (var data in dataReader.ReadAll<TResult>())
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
                    })));
        }


        public static IObservable<T> WriteOutput<T>(this IObservable<T> data, BaseOptions options)
        {
            return WriteOutput<T>(data, options.OutputFile, options.OutputFileType);
        }


        public static IObservable<T> WriteOutput<T>(this IObservable<T> data, string outputFile, OutputFileType outputFileType)
        {
            return Observable.Using(() => CreateOutputWriter(outputFile),
                (writer) => Observable.Using(() => GetOutputDataWriter<T>(writer, outputFileType),
                    (dataWriter) => data.Finally(dataWriter.Close).Do(d => dataWriter.Write(d))));
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
}
