using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Serialization
{
    public interface IDataWriter : IDisposable
    {
        bool CanWrite(Type objectType);

        void Write(object data);

        void Close();

        void WriteRange(IEnumerable<object> data);
    }
}
