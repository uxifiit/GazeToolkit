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
    public enum FileFormat
    {
        JSON, CSV, TSV
    }

    //public enum InputFileType
    //{
    //    JSON,
    //    CSV,  // not supported yet
    //}


    //public enum OutputFileType
    //{
    //    CSV, // not supported
    //    TSV, // not supported
    //    JSON
    //}


    public class FilterIO
    {
        public FilterIO(IEnumerable<IDataSerializationFactory> factories)
        {

        }
        

        public void Initialize(BaseOptions options)
        {

        }
    }



    public static class FilterIOEx
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


        private static IDataReader GetInputDataReader(TextReader reader, FileFormat fileType, Type dataType)
        {
            IDataSerializationFactory factory = formats.FirstOrDefault(f => f.Format == fileType);

            if (factory == null)
            {
                throw new ArgumentOutOfRangeException(nameof(fileType));
            }

            return factory.CreateReaderForType(reader, dataType);
        }


        private static IDataWriter GetOutputDataWriter(TextWriter writer, FileFormat fileType, Type dataType)
        {
            var factory = formats.FirstOrDefault(f => f.Format == fileType);

            if (factory == null)
            {
                throw new ArgumentOutOfRangeException(nameof(fileType));
            }

            return factory.CreateWriterForType(writer, dataType);
        }


        public static IObservable<object> ReadInput(BaseOptions options, Type dataType)
        {
            return ReadInput(options.InputFile, options.InputFileType, dataType);
        }

        //public static IObservable<TResult> ReadInput<TResult>(BaseOptions options)
        //{
        //    return ReadInput(options.InputFile, options.InputFileType, typeof(TResult)).OfType<TResult>();
        //}


        public static IObservable<object> ReadInput(string inputFile, FileFormat inputFileType, Type dataType)
        {
            return Observable.Using(() => CreateInputReader(inputFile), (reader) =>
            {
                return Observable.Using(() => GetInputDataReader(reader, inputFileType, dataType), (dataReader) =>
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


        public static IObservable<object> WriteOutput(this IObservable<object> data, BaseOptions options, Type dataType)
        {
            return WriteOutput(data, options.OutputFile, options.OutputFileType, dataType);
        }

        
        public static IObservable<object> WriteOutput(this IObservable<object> data, string outputFile, FileFormat outputFileType, Type dataType)
        {
            // similar to nested using statements, but managed by the lifetime of observable
            //
            // using (TextWriter writer = CreateOutputWriter(outputFile)) 
            // using (IDataWriter dataWriter = GetOutputDataWriter<T>(writer, outputFileType)) 
            // ...

            return Observable.Using(() => CreateOutputWriter(outputFile), (TextWriter writer) =>
            {
                return Observable.Using(() => GetOutputDataWriter(writer, outputFileType, dataType), (IDataWriter dataWriter) =>
                {
                    return data.Finally(dataWriter.Close)
                               .Do(d => dataWriter.Write(d));
                });
            });
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
