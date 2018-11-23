using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Serialization.Converters
{
    public enum Precision : int
    {
        HundredNanoseconds = 1,
        Microsecond = 10,
        Millisecond = Microsecond * 1000
    }


    public class TimestampFromTicksConverter : ITimestampStringConverter
    {
        public const Precision DefaultPrecision = Precision.HundredNanoseconds;

        public TimestampFromTicksConverter()
        {
        }


        public Precision Precision { get; set; } = DefaultPrecision;


        public bool IsUsingDefaultFormat => Precision == DefaultPrecision;

        
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

        // TODO add support for ticks with floating point
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
    }
}
