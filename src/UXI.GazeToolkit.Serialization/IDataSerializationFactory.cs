using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Serialization
{
    public interface IDataSerializationFactory
    {
        FileFormat Format { get; }

        IDataWriter CreateWriterForType(TextWriter writer, Type dataType, SerializationConfiguration configuration);

        IDataReader CreateReaderForType(TextReader reader, Type dataType, SerializationConfiguration configuration);
    }
}
