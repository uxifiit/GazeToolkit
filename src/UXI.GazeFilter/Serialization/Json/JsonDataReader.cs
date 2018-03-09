using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UXI.GazeFilter.Common;

namespace UXI.GazeFilter.Serialization.Json
{
    public class JsonDataReader : DisposableBase, IDataReader, IDisposable
    {
        private readonly JsonSerializer _serializer;
        private readonly JsonTextReader _reader;

        public JsonDataReader(TextReader reader, Type dataType, JsonSerializer serializer)
        {
            _serializer = serializer;
            _reader = new JsonTextReader(reader);
            DataType = dataType;
        }


        public Type DataType { get; }


        public bool CanRead(Type objectType)
        {
            return _serializer.Converters.Any(c => c.CanConvert(objectType));
        }


        public bool TryRead(out object data)
        {
            while (_reader.Read())
            {
                if (_reader.TokenType == JsonToken.StartObject)
                {
                    JObject obj = JObject.Load(_reader);
                    data = obj.ToObject(DataType, _serializer);
                    return true;
                }
            }
            data = null;
            return false;
        }


        private bool _disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    _reader.Close();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
