using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UXI.GazeFilter.Serialization.Json
{
    public class JsonSerializationFactory : IDataSerializationFactory
    {
        private readonly JsonSerializer _serializer;

        public JsonSerializationFactory(IEnumerable<JsonConverter> converters)
        {
            _serializer = new JsonSerializer();
            converters.ToList().ForEach(_serializer.Converters.Add);
        }


        public string FileExtension => "json";

        public string FormatName => "JSON";

        public string MimeType => "application/json";


        public IDataReader CreateReaderForType(TextReader reader, Type dataType)
        {
            return new JsonDataReader(reader, dataType, _serializer);
        }


        public IDataWriter CreateWriterForType(TextWriter writer, Type dataType)
        {
            return new JsonDataWriter(writer, _serializer);
        }
    }
}
