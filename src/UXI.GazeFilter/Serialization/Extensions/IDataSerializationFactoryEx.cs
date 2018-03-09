using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Data.Serialization.Extensions
{
    public static class IDataSerializationFactoryEx
    {
        public static IDataWriter CreateWriter<TData>(this IDataSerializationFactory factory, TextWriter writer)
        {
            return factory.CreateWriterForType(writer, typeof(TData));
        }
    }
}
