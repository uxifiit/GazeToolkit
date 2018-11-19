using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Serialization.Converters
{
    public class TimestampFromDateTimeConverter : ITimestampStringConverter
    {
        private string _format = null;

        public void Configure(string format)
        {
            _format = format;
        }


        public bool IsUsingDefaultFormat => String.IsNullOrWhiteSpace(_format);


        public DateTimeOffset Convert(string value)
        {
            DateTimeOffset datetime;

            if ((IsUsingDefaultFormat && DateTimeOffset.TryParse(value, out datetime))
                || DateTimeOffset.TryParseExact(value, _format, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out datetime))
            {
                return datetime;                
            }

            throw new ArgumentException($"Failed to parse date and time from string '{value}'.");
            //DateTime datetime = DateTime.Parse(dateTimeString, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            //return new DateTimeOffset(datetime, TimeZoneInfo.Local.GetUtcOffset(datetime));
            //return DateTimeOffset.Parse(value);
        }


        public string ConvertBack(DateTimeOffset value)
        {
            return IsUsingDefaultFormat
                 ? value.ToString("o")
                 : value.ToString(_format);
        }
    }
}
