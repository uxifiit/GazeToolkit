using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UXI.GazeToolkit.Serialization.Common;

namespace UXI.GazeToolkit.Serialization.Json
{
    public class JsonDataWriter : DisposableBase, IDataWriter, IDisposable
    {
        private readonly JsonSerializer _serializer;
        private readonly JsonTextWriter _writer;
        private bool _isOpen = true;

        private ConcurrentQueue<object> _cache = new ConcurrentQueue<object>();

        public JsonDataWriter(TextWriter writer, JsonSerializer serializer)
        {
            _serializer = serializer;

            _writer = new JsonTextWriter(writer)
            {
                Culture = serializer.Culture,
                DateTimeZoneHandling = serializer.DateTimeZoneHandling,
                DateFormatHandling = serializer.DateFormatHandling,
                AutoCompleteOnClose = true
            };
        }


        public bool CanWrite(Type objectType)
        {
            return _isOpen;
        }


        public void Close()
        {
            if (_isOpen)
            {
                object cachedData;
                if (_cache != null && _cache.TryDequeue(out cachedData))
                {
                    _cache = null;

                    _serializer.Serialize(_writer, cachedData);
                }

                _isOpen = false;
                _writer.Flush();
            }
            _writer.Close();
        }


        public void Write(object data)
        {
            if (_isOpen)
            {
                if (_cache != null)
                {
                    object cachedData = null;
                    if (_cache?.IsEmpty == true)
                    {
                        _cache.Enqueue(data);
                        return;
                    }
                    else if (_cache?.TryDequeue(out cachedData) == true)
                    {
                        _cache = null;

                        _writer.WriteStartArray();

                        _serializer.Serialize(_writer, cachedData);
                    }
                }

                _serializer.Serialize(_writer, data);
            }
        }
        

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    Close();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
