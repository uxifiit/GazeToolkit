using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Serialization.Converters
{
    public class TimestampStringConverterResolver
    {
        public static TimestampStringConverterResolver Default { get; } = new TimestampStringConverterResolver();

        public ITimestampStringConverter Resolve(string format)
        {
            string[] formatParts = format?.Split(new char[] { ':' }, count: 2, options: StringSplitOptions.RemoveEmptyEntries);

            string converterId = formatParts?.FirstOrDefault();
            string configuration = formatParts?.Skip(1).FirstOrDefault();

            ITimestampStringConverter converter = String.IsNullOrWhiteSpace(converterId)
                                                ? CreateDefaultConverter()
                                                : ResolveConverterFromId(converterId);

            // if was not resolved and no configuration is provided but id has value,
            // value of the converterId may be the configuration for the default converter.
            if (converter == null 
                && String.IsNullOrWhiteSpace(configuration)
                && String.IsNullOrWhiteSpace(converterId) == false)
            {
                converter = CreateDefaultConverter();
                configuration = converterId;
            }

            if (String.IsNullOrWhiteSpace(configuration) == false)
            {
                converter.Configure(configuration);
            }

            return converter;
        }


        private ITimestampStringConverter ResolveConverterFromId(string converterId)
        { 
            switch (converterId.ToLower())
            {
                case "d":
                case "dt":
                case "date":
                    return new TimestampFromDateTimeConverter();
                case "t":
                case "tm":
                case "time":
                    return new TimestampFromTimeSpanConverter();
                case "c":
                case "k":
                case "tick":
                case "ticks":
                    return new TimestampFromTicksConverter();
                default:
                    return null;
            }
        }


        private ITimestampStringConverter CreateDefaultConverter()
        {
            return new TimestampFromTicksConverter();
        }
    }
}
