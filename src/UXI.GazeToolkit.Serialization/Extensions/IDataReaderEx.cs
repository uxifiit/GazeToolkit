using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Serialization.Extensions
{
    public static class IDataReaderEx
    {
        public static IEnumerable<T> ReadAll<T>(this IDataReader reader)
        {
            if (reader.CanRead(typeof(T)))
            {
                object data;
                while (reader.TryRead(out data))
                {
                    yield return (T)data;
                }
            }
        }
    }
}
