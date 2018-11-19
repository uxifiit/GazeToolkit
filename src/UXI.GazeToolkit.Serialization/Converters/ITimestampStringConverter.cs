using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Serialization.Converters
{
    public interface ITimestampStringConverter
    {
        void Configure(string format);

        bool IsUsingDefaultFormat { get; }

        DateTimeOffset Convert(string value);

        string ConvertBack(DateTimeOffset value);
    }
}
