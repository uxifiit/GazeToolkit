using System;
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

        public JsonDataWriter(TextWriter writer, JsonSerializer serializer)
        {
            _serializer = serializer;

            _writer = new JsonTextWriter(writer);

            _writer.Culture = serializer.Culture;
            _writer.DateTimeZoneHandling = serializer.DateTimeZoneHandling;
            _writer.DateFormatHandling = serializer.DateFormatHandling;

            _writer.WriteStartArray();

            _writer.AutoCompleteOnClose = true;
        }


        public bool CanWrite(Type objectType)
        {
            return _isOpen; 
        }


        public void Close()
        {
            if (_isOpen)
            {
                _isOpen = false;
                _writer.WriteEndArray();
                _writer.Flush();
            }
            _writer.Close();
        }


        public void Write(object data)
        {
            if (_isOpen)
            {
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
