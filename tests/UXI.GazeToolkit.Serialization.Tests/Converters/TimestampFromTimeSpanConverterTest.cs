using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UXI.GazeToolkit.Serialization.Converters;

namespace UXI.GazeToolkit.Serialization.Converters
{
    [TestClass]
    public class TimestampFromTimeSpanConverterTest
    {
        public TimestampFromTimeSpanConverterTest()
        {
        }


        [TestMethod]
        public void IsUsingDefaultFormat_TrueForNewInstance()
        {
            var converter = new TimestampFromTimeSpanConverter();

            Assert.IsTrue(converter.IsUsingDefaultFormat);
        }


        [TestMethod]
        public void IsUsingDefaultFormat_FalseAfterConfigure()
        {
            var converter = new TimestampFromTimeSpanConverter();

            converter.Configure(@"d\.hh\:mm\:ss");

            Assert.IsFalse(converter.IsUsingDefaultFormat);
        }


        [TestMethod]
        public void Configure_ResetFormatWithNullOrEmptyString()
        {
            var converter = new TimestampFromTimeSpanConverter();

            converter.Configure(@"d\.hh\:mm\:ss");

            converter.Configure(String.Empty);

            Assert.IsTrue(converter.IsUsingDefaultFormat);
        }


        [TestMethod]
        public void Convert_ValidValue()
        {
            var converter = new TimestampFromTimeSpanConverter();

            var expected = DateTimeOffset.MinValue.Add(new TimeSpan(((((14L * 60 + 30) * 60 + 20) * 1000 + 40) * 1000 + 555) * 10));

            DateTimeOffset timestamp = converter.Convert("14:30:20.040555");

            Assert.AreEqual(expected, timestamp);
        }


        [TestMethod]
        public void Convert_ValidValue_CustomFormat()
        {
            var converter = new TimestampFromTimeSpanConverter();
            converter.Configure(@"hh\-mm\-ss\.ffffff");

            var expected = DateTimeOffset.MinValue.Add(new TimeSpan(((((14L * 60 + 30) * 60 + 20) * 1000 + 40) * 1000 + 555) * 10));

            DateTimeOffset timestamp = converter.Convert("14-30-20.040555");

            Assert.AreEqual(expected, timestamp);
        }


        [TestMethod]
        public void Convert_InvalidValue()
        {
            var converter = new TimestampFromTimeSpanConverter();

            try
            {
                DateTimeOffset timestamp = converter.Convert("14-30-20.040555");

                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }


        [TestMethod]
        public void ConvertBack_ValidValue_TimeOnly()
        {
            var converter = new TimestampFromTimeSpanConverter();

            string expected = "14:30:20.0405550";
            var timestamp = DateTimeOffset.MinValue.Add(new TimeSpan(((((14L * 60 + 30) * 60 + 20) * 1000 + 40) * 1000 + 555) * 10));

            string timestampString = converter.ConvertBack(timestamp);

            Assert.AreEqual(expected, timestampString);
        }


        [TestMethod]
        public void ConvertBack_ValidValue_IgnoreDatePart()
        {
            var converter = new TimestampFromTimeSpanConverter();

            string expected = "14:30:20.0405550";
            var timestamp = new DateTimeOffset(new DateTime(2018, 3, 24)).Add(new TimeSpan(((((14L * 60 + 30) * 60 + 20) * 1000 + 40) * 1000 + 555) * 10));

            string timestampString = converter.ConvertBack(timestamp);

            Assert.AreEqual(expected, timestampString);
        }


        [TestMethod]
        public void ConvertBack_ValidValue_CustomFormat()
        {
            var converter = new TimestampFromTimeSpanConverter();

            converter.Configure(@"hh\-mm\-ss\.fff");

            string expected = "14-30-20.040";
            var timestamp = new DateTimeOffset(new DateTime(2018, 3, 24)).Add(new TimeSpan(((((14L * 60 + 30) * 60 + 20) * 1000 + 40) * 1000 + 555) * 10));

            string timestampString = converter.ConvertBack(timestamp);

            Assert.AreEqual(expected, timestampString);
        }


        [TestMethod]
        public void ConvertAndConvertBack_ValidValue()
        {
            var converter = new TimestampFromTimeSpanConverter();

            string inputString = "14:30:20.0405550";
            var expected = DateTimeOffset.MinValue.Add(new TimeSpan(((((14L * 60 + 30) * 60 + 20) * 1000 + 40) * 1000 + 555) * 10));

            DateTimeOffset timestamp = converter.Convert(inputString);
            string timestampString = converter.ConvertBack(timestamp);

            Assert.AreEqual(expected, timestamp);
            Assert.AreEqual(inputString, timestampString);
        }
    }
}
