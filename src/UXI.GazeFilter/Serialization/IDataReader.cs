using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Data.Serialization
{
    public interface IDataReader : IDisposable
    {
        bool CanRead(Type objectType);

        bool TryRead(out object data);
    }
}
