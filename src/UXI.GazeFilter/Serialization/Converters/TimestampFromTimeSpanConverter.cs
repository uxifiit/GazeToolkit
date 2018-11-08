using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter.Serialization.Converters
{
    class TimestampFromTimeSpanConverter : ITimestampStringConverter
    {
        public DateTimeOffset Convert(string value)
        {
            var timespan = TimeSpan.Parse(value);

            return new DateTimeOffset(timespan.Ticks, TimeZoneInfo.Utc.BaseUtcOffset);
        }


        public string ConvertBack(DateTimeOffset value)
        {
            var timespan = value.TimeOfDay;

            return timespan.ToString(); // TODO format
        }
    }
}
