using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter.Serialization.Converters
{
    public class TimestampFromDateTimeConverter : ITimestampStringConverter
    {
        public DateTimeOffset Convert(string value)
        {
            //DateTime datetime = DateTime.Parse(dateTimeString, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            //return new DateTimeOffset(datetime, TimeZoneInfo.Local.GetUtcOffset(datetime));
            return DateTimeOffset.Parse(value);
        }

        public string ConvertBack(DateTimeOffset value)
        {
            return value.ToString("o");
        }
    }
}
