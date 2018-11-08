using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter.Serialization.Converters
{
    public interface ITimestampStringConverter
    {
        DateTimeOffset Convert(string value);
        string ConvertBack(DateTimeOffset value);
    }
}
