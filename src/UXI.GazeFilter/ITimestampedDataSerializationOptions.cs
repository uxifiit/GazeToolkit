using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter
{
    public interface ITimestampedDataSerializationOptions
    {
        string TimestampFieldName { get; set; }
    }
}
