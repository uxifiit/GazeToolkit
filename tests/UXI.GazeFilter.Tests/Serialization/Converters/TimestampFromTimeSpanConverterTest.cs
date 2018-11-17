using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UXI.GazeFilter.Serialization.Converters;

namespace UXI.GazeFilter.Tests.Serialization.Converters
{
    [TestClass]
    public class TimestampFromTimeSpanConverterTest
    {
        public TimestampFromTimeSpanConverterTest()
        {
        }


        [TestMethod]
        public void TestIsUsingDefaultFormat_TrueForNewInstance()
        {
            var converter = new TimestampFromTimeSpanConverter();

            Assert.IsTrue(converter.IsUsingDefaultFormat);
        }


        [TestMethod]
        public void TestIsUsingDefaultFormat_FalseAfterConfigure()
        {
            var converter = new TimestampFromTimeSpanConverter();

            converter.Configure(@"d\.hh\:mm\:ss");

            Assert.IsFalse(converter.IsUsingDefaultFormat);
        }


        [TestMethod]
        public void TestConfigure_ResetFormatWithNullOrEmptyString()
        {
            var converter = new TimestampFromTimeSpanConverter();

            converter.Configure(@"d\.hh\:mm\:ss");

            converter.Configure(String.Empty);

            Assert.IsTrue(converter.IsUsingDefaultFormat);
        }


        [TestMethod]
        public void TestConvert_ValidValue()
        {
            var converter = new TimestampFromTimeSpanConverter();

            DateTimeOffset datetime = converter.Convert("14:30:20.040555");

            TimeSpan expected = new TimeSpan(((((14L * 60 + 30) * 60 + 20) * 1000 + 40) * 1000 + 555) * 10);

            Assert.AreEqual(expected, datetime.TimeOfDay);
        }


        [TestMethod]
        public void TestConvert_ValidValue_CustomFormat()
        {
            var converter = new TimestampFromTimeSpanConverter();


        }


        [TestMethod]
        public void TestConvert_InvalidValue()
        {

        }
    }
}
