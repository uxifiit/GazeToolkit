using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using UXI.GazeToolkit.Serialization.Common;
using UXI.GazeToolkit.Serialization.Csv.DataConverters;

namespace UXI.GazeToolkit.Serialization.Csv
{
    public class CsvDataReader : DisposableBase, IDataReader, IDisposable
    {
        private readonly CsvReader _reader;
        private readonly CsvSerializerContext _serializer;
        private readonly IDataConverter _converter;
        private bool _shouldReadHeader = true;

        public CsvDataReader(TextReader reader, Type dataType, CsvSerializerContext serializer)
        {
            _reader = new CsvReader(reader, serializer.Configuration);
            _serializer = serializer;

            _converter = serializer.DataConverters.FirstOrDefault(c => c.CanConvert(dataType)); // TODO ?? DefaultCsvConverter ?

            DataType = dataType;
        }


        public Type DataType { get; }

        // TODO not in converters, will never work if object of other type is passed because no converter will be found
        public bool CanRead(Type objectType)
        {
            return DataType == objectType
                || DataType.IsSubclassOf(objectType)
                || objectType.IsAssignableFrom(DataType);
        }


        public bool TryRead(out object data)
        {
            TryReadHeader();

            try
            {
                if (_reader.Read())
                {
                    data = _converter.ReadCsv(_reader, DataType, _serializer);
                    return true;
                }
            }
            catch (Exception exception)
            {
                throw new System.Runtime.Serialization.SerializationException("Failed to read or deserialize next data from the CSV file. See inner exception for more details.", exception);
            }

            data = null;
            return false;
        }


        private void TryReadHeader()
        {
            if (_shouldReadHeader)
            {
                try
                {
                    if (_reader.Read())
                    {
                        _converter.ReadCsvHeader(_reader, _serializer);
                        _shouldReadHeader = false;
                    }
                }
                catch (CsvHelperException exception)
                {
                    throw new System.Runtime.Serialization.SerializationException("Failed to read CSV file header. See inner exception for more details.", exception);
                }
            }
        }


        private bool _disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    _reader.Dispose();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
