using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter.Serialization.Converters
{
    public enum Precision : int
    {
        HundredNanoseconds = 1,
        Microsecond = 10,
        Millisecond = Microsecond * 1000
    }


    public class TimestampFromTicksConverter : ITimestampStringConverter
    {
        public TimestampFromTicksConverter()
        {
        }


        public Precision Precision { get; set; } = Precision.HundredNanoseconds;

        
        public void Configure(string format)
        {
            Precision = ResolvePrecision(format);
        }


        private Precision ResolvePrecision(string value)
        {
            switch (value.ToLower())
            {
                case "us":
                    return Precision.Microsecond;
                case "ms":
                    return Precision.Millisecond;
                case "ns":
                case "100ns":
                    return Precision.HundredNanoseconds;
                default:
                    throw new ArgumentOutOfRangeException($"Failed to resolve supported precision for timestamp ticks for '{value}'.");
            }
        }


        public DateTimeOffset Convert(string value)
        {
            long ticks = Int64.Parse(value);

            ticks *= (long)Precision;

            // TODO
            // check size and increase/decrease to .NET DateTime ticks

            return new DateTimeOffset(ticks, TimeZoneInfo.Utc.BaseUtcOffset);
        }


        public string ConvertBack(DateTimeOffset value)
        {
            long ticks = value.UtcTicks;

            if (Precision != Precision.HundredNanoseconds)
            {
                ticks = ChangeTicksPrecision(ticks, Precision);
            }

            return ticks.ToString();
        }


        private static long ChangeTicksPrecision(long ticks, Precision newPrecision)
        {
            long precision = (long)newPrecision;
            long newTicks = ticks / precision;
            long remainder = ticks % precision;

            if (remainder > 0 && remainder >= precision / 2)
            {
                newTicks += 1;
            }

            return newTicks;
        }


        //JToken tokenTrackerTicks;
        //JToken tokenTimestamp;

        //bool hasTrackerTicks = obj.TryGetValue(nameof(TimestampedData.Timestamp), out tokenTrackerTicks);
        //bool hasTimestamp = obj.TryGetValue(/*nameof(TimestampedData.Timestamp)*/"Timestamp", out tokenTimestamp);

        //if (hasTrackerTicks && hasTimestamp)
        //{
        //    trackerTicks = tokenTrackerTicks.ToObject<long>(serializer);
        //    timestamp = tokenTimestamp.ToObject<TimeSpan>(serializer);
        //}
        //else if (hasTimestamp)
        //{
        //    timestamp = tokenTimestamp.ToObject<TimeSpan>(serializer);
        //    trackerTicks = timestamp.Ticks / 10;
        //}
        //else if (hasTrackerTicks)
        //{
        //    trackerTicks = tokenTrackerTicks.ToObject<long>(serializer);
        //    timestamp = TimeSpan.Zero;
        //}
        //else
        //{
        //    trackerTicks = 0L;
        //    timestamp = TimeSpan.Zero;
        //}

        //return hasTrackerTicks || hasTimestamp;

    }
}
