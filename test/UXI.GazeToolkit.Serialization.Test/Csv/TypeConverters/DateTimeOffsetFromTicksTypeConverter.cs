using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace UXI.GazeToolkit.Serialization.Csv.TypeConverters
{
    public class DateTimeOffsetFromTicksTypeConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            long ticks = Int64.Parse(text);

            return new DateTimeOffset(ticks, TimeZoneInfo.Utc.BaseUtcOffset);
        }


        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            long ticks = ((DateTimeOffset)value).UtcTicks;

            return ticks.ToString();
        }
    }
}
