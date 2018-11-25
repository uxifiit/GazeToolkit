using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using UXI.GazeToolkit.Serialization.Common;
using UXI.GazeToolkit.Serialization.Csv.DataConverters;

namespace UXI.GazeToolkit.Serialization.Csv
{
    public class CsvDataWriter : DisposableBase, IDataWriter, IDisposable
    {
        private readonly CsvWriter _writer;
        private readonly CsvSerializerContext _serializer;
        private readonly IDataConverter _converter;
        private bool _isOpen = true;

        private bool _shouldWriteHeader = true;
    
        public CsvDataWriter(TextWriter writer, Type dataType, CsvSerializerContext serializer)
        {
            _writer = new CsvWriter(writer, serializer.Configuration);
            
            _serializer = serializer;

            _converter = serializer.DataConverters.FirstOrDefault(c => c.CanConvert(dataType));

            DataType = dataType;
        }


        public Type DataType { get; }


        public bool CanWrite(Type objectType)
        {
            return _isOpen 
                && (DataType == objectType
                    || objectType.IsSubclassOf(DataType)
                    || DataType.IsAssignableFrom(objectType));
        }


        public void Write(object data)
        {
            TryWriteHeader();

            if (_isOpen)
            {
                try
                {
                    _converter.WriteCsv(data, _writer, _serializer);
                }
                catch (Exception exception)
                {
                    throw new SerializationException($"Failed to write or serialize next data to the CSV file. See inner exception for more details.", exception);
                }
            }
        }


        private void TryWriteHeader()
        {
            if (_shouldWriteHeader)
            {
                try
                {

                _converter.WriteCsvHeader(_writer, DataType, _serializer);
                }
                catch (Exception ex)
                {

                }
                _shouldWriteHeader = false;
            }
        }


        public void Close()
        {
            if (_isOpen)
            {
                TryWriteHeader();
                _isOpen = false;
                _writer.Flush();
            }
        }


        private bool _disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    _writer.Dispose();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
