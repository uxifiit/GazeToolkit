using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Serialization
{
    public class DataIO
    {
        private readonly Dictionary<FileFormat, IDataSerializationFactory> _formats;

        public DataIO(IEnumerable<IDataSerializationFactory> factories)
        {
            _formats = factories.ToDictionary(f => f.Format);
        }


        public IEnumerable<object> ReadInput(string filePath, FileFormat fileFormat, Type dataType, SerializationConfiguration configuration)
        {
            FileFormat format = EnsureCorrectFileFormat(filePath, fileFormat);

            using (var reader = FileHelper.CreateInputReader(filePath))
            using (var dataReader = GetInputDataReader(reader, format, dataType, configuration))
            {
                object data;
                while (dataReader.TryRead(out data))
                {
                    yield return data;
                }

                yield break;
            }
        }


        public IDataReader GetInputDataReader(TextReader reader, FileFormat fileType, Type dataType, SerializationConfiguration configuration)
        {
            IDataSerializationFactory factory;

            if (_formats.TryGetValue(fileType, out factory))
            {
                return factory.CreateReaderForType(reader, dataType, configuration);
            }

            throw new ArgumentOutOfRangeException(nameof(fileType));
        }


     
        public void WriteOutput(IEnumerable<object> data, string filePath, FileFormat fileFormat, Type dataType, SerializationConfiguration configuration)
        {
            FileFormat format = EnsureCorrectFileFormat(filePath, fileFormat);

            using (var writer = FileHelper.CreateOutputWriter(filePath))
            using (var dataWriter = GetOutputDataWriter(writer, format, dataType, configuration))
            {
                foreach (var item in data)
                {
                    dataWriter.Write(item);
                }

                dataWriter.Close();
            }
        }


        public IDataWriter GetOutputDataWriter(TextWriter writer, FileFormat fileType, Type dataType, SerializationConfiguration configuration)
        {
            IDataSerializationFactory factory;

            if (_formats.TryGetValue(fileType, out factory))
            {
                return factory.CreateWriterForType(writer, dataType, configuration);
            }

            throw new ArgumentOutOfRangeException(nameof(fileType));
        }


        public FileFormat EnsureCorrectFileFormat(string filename, FileFormat requestedFormat)
        {
            string extension = Path.GetExtension(filename)?.ToLower();

            if (String.IsNullOrWhiteSpace(extension) == false)
            {
                var matchingFormat = _formats.Where(f => f.Key.ToString().ToLower() == extension)
                                             .Select(f => f.Value)
                                             .FirstOrDefault();

                return matchingFormat != null && matchingFormat.Format != requestedFormat
                     ? matchingFormat.Format
                     : requestedFormat;
            }

            return requestedFormat;
        }
    }
}
