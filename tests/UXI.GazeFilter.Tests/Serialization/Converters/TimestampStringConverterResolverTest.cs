using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UXI.GazeFilter.Serialization.Converters;

namespace UXI.GazeFilter.Tests.Serialization.Converters
{
    [TestClass]
    public class TimestampStringConverterResolverTest
    {
        private readonly IEnumerable<string> TimeFormatAliases = new string[] { "time", "tm", "t" };
        private readonly IEnumerable<string> DateFormatAliases = new string[] { "date", "dt", "d" };
        private readonly IEnumerable<string> TicksFormatAliases = new string[] { "ticks", "tick", "c", "k" };

        [TestMethod]
        public void Resolve_TimeFormatAliases()
        {
            var resolver = new TimestampStringConverterResolver();

            foreach (var alias in TimeFormatAliases)
            {
                var converter = resolver.Resolve(alias);

                Assert.IsInstanceOfType(converter, typeof(TimestampFromTimeSpanConverter), $"Incorrect type of the converter for the format: {alias}");
                Assert.IsTrue(converter.IsUsingDefaultFormat);
            }
        }


        [TestMethod]
        public void Resolve_DateFormatAliases()
        {
            var resolver = new TimestampStringConverterResolver();

            foreach (var alias in DateFormatAliases)
            {
                var converter = resolver.Resolve(alias);

                Assert.IsInstanceOfType(converter, typeof(TimestampFromDateTimeConverter), $"Incorrect type of the converter for the format: {alias}");
                Assert.IsTrue(converter.IsUsingDefaultFormat);
            }
        }


        [TestMethod]
        public void Resolve_TicksFormatAliases()
        {
            var resolver = new TimestampStringConverterResolver();

            foreach (var alias in TicksFormatAliases)
            {
                var converter = resolver.Resolve(alias);

                Assert.IsInstanceOfType(converter, typeof(TimestampFromTicksConverter), $"Incorrect type of the converter for the format: {alias}");
                Assert.IsTrue(converter.IsUsingDefaultFormat);
            }
        }


        [TestMethod]
        public void Resolve_DefaultTicksConverterForNoFormat()
        {
            var resolver = new TimestampStringConverterResolver();

            var converter = resolver.Resolve(null);

            Assert.IsInstanceOfType(converter, typeof(TimestampFromTicksConverter));
            Assert.IsTrue(converter.IsUsingDefaultFormat);
        }


        [TestMethod]
        public void Resolve_OnlyFormatForDefaultTicksConverter()
        {
            var resolver = new TimestampStringConverterResolver();

            var converter = resolver.Resolve("ms");

            Assert.IsInstanceOfType(converter, typeof(TimestampFromTicksConverter));
            Assert.IsFalse(converter.IsUsingDefaultFormat);

            string stringValue = "345";
            var testValue = DateTimeOffset.MinValue.AddMilliseconds(345);

            Assert.AreEqual(testValue, converter.Convert(stringValue));
            Assert.AreEqual(stringValue, converter.ConvertBack(testValue));
        }


        [TestMethod]
        public void Resolve_TimeWithCustomFormat()
        {
            var resolver = new TimestampStringConverterResolver();

            string stringValue = "3.14:30:20.040555";
            var testValue = DateTimeOffset.MinValue.Add(new TimeSpan(3, 14, 30, 20, 040)).AddTicks(5550);
            
            foreach (var alias in TimeFormatAliases)
            {
                var converter = resolver.Resolve($"{alias}:d\\.hh\\:mm\\:ss\\.ffffff");

                Assert.IsFalse(converter.IsUsingDefaultFormat);

                Assert.AreEqual(testValue, converter.Convert(stringValue));
                Assert.AreEqual(stringValue, converter.ConvertBack(testValue));
            }
        }


        [TestMethod]
        public void Resolve_TicksWithCustomFormat()
        {
            var resolver = new TimestampStringConverterResolver();

            long microsecondTicksValue = 311420040555L;
            var testValue = DateTimeOffset.MinValue.AddTicks(microsecondTicksValue * 10);
            string stringValue = microsecondTicksValue.ToString();

            foreach (var alias in TicksFormatAliases)
            {
                var converter = resolver.Resolve($"{alias}:us");

                Assert.IsFalse(converter.IsUsingDefaultFormat);

                Assert.AreEqual(testValue, converter.Convert(stringValue));
                Assert.AreEqual(stringValue, converter.ConvertBack(testValue));
            }
        }
    }
}
