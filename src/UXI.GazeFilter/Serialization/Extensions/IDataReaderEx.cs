using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter.Serialization.Extensions
{
    public static class IDataReaderEx
    {
        public static bool TryRead<T>(this IDataReader reader, out T data)
        {
            return reader.TryRead(out data);
        }

        public static IEnumerable<object> ReadAll(this IDataReader reader, Type objectType)
        {
            if (reader.CanRead(objectType))
            {
                object data;
                while (reader.TryRead(out data))
                {
                    yield return data;
                }
            }
        }

        public static IEnumerable<T> ReadAll<T>(this IDataReader reader)
        {
            return ReadAll(reader, typeof(T)).OfType<T>();
        }
    }
}
