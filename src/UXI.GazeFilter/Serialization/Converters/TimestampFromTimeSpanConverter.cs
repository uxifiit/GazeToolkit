using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter.Serialization.Converters
{
    public class TimestampFromTimeSpanConverter : ITimestampStringConverter
    {
        private string _format = null;

        public void Configure(string format)
        {
            _format = format;
        }


        public bool IsUsingDefaultFormat => String.IsNullOrWhiteSpace(_format);


        public DateTimeOffset Convert(string value)
        {
            TimeSpan timespan;
            if ((IsUsingDefaultFormat && TimeSpan.TryParse(value, out timespan))
                || TimeSpan.TryParseExact(value, _format, System.Globalization.CultureInfo.CurrentCulture, out timespan))
            {
                return new DateTimeOffset(timespan.Ticks, TimeZoneInfo.Utc.BaseUtcOffset);
            }

            throw new ArgumentException($"Failed to parse time from string '{value}'.");
        }


        public string ConvertBack(DateTimeOffset value)
        {
            var timespan = TimeSpan.FromTicks(value.Ticks);

            return IsUsingDefaultFormat
                 ? value.TimeOfDay.ToString()
                 : timespan.ToString(_format);
        }
    }
}
