using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Data.Serialization
{
    public interface IDataSerializationFactory
    {
        string FileExtension { get; }

        string FormatName { get; }

        string MimeType { get; }

        IDataWriter CreateWriterForType(TextWriter writer, Type dataType);

        IDataReader CreateReaderForType(TextReader reader, Type dataType);
    }
}
