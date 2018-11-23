using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using UXI.GazeToolkit.Serialization.Converters;
using UXI.GazeToolkit.Validation;
using System.Linq;
using System.Runtime.Serialization;
using UXI.GazeToolkit.Serialization.Extensions;
using UXI.GazeFilter.Validation.Serialization.Csv.DataConverters;
using UXI.GazeToolkit.Serialization.Csv.TypeConverters;
using UXI.GazeToolkit.Serialization.Csv;

namespace UXI.GazeFilter.Validation.Serialization.Csv
{
    /// <summary>
    /// Summary description for ValidationPointCsvReaderTest
    /// </summary>
    [TestClass]
    public class ValidationPointCsvReaderTest
    {
        public ValidationPointCsvReaderTest()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ReadAll_RecordsWithDateTimestamp()
        {
            var timestampConverter = TimestampStringConverterResolver.Default.Resolve("date");
            var context = new CsvSerializerContext() { TimestampConverter = timestampConverter };
            context.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetTypeConverter(timestampConverter));
            context.DataConverters.Add(new ValidationPointDataConverter());

            DateTimeOffset startTimestamp = new DateTimeOffset(2018, 11, 20, 12, 08, 28, 477, TimeSpan.FromHours(2));
            DateTimeOffset startA = startTimestamp;
            DateTimeOffset endA = startTimestamp.AddSeconds(1);

            DateTimeOffset startB = endA.AddSeconds(2);
            DateTimeOffset endB = startB.AddSeconds(1);

            DateTimeOffset startC = endB.AddSeconds(2);
            DateTimeOffset endC = startC.AddSeconds(1);

            string[] lines = new[]
            {
                "Validation,Point,X,Y,StartTime,EndTime"
            ,  $"1,1,0.1,0.1,{context.TimestampConverter.ConvertBack(startA)},{context.TimestampConverter.ConvertBack(endA)}"
            ,  $"2,1,0.5,0.9,{context.TimestampConverter.ConvertBack(startB)},{context.TimestampConverter.ConvertBack(endB)}"
            ,  $"3,1,0.9,0.1,{context.TimestampConverter.ConvertBack(startC)},{context.TimestampConverter.ConvertBack(endC)}"
            };

            string input = String.Join(Environment.NewLine, lines);

            using (var inputReader = new StringReader(input))
            using (var reader = new CsvDataReader(inputReader, typeof(ValidationPoint), context))
            {
                var records = reader.ReadAll<ValidationPoint>().ToList();

                Assert.AreEqual(3, records.Count);

                AssertRecord(new ValidationPoint(1, new GazeToolkit.Point2(0.1, 0.1), startA, endA), records[0]);
                AssertRecord(new ValidationPoint(2, new GazeToolkit.Point2(0.5, 0.9), startB, endB), records[1]);
                AssertRecord(new ValidationPoint(3, new GazeToolkit.Point2(0.9, 0.1), startC, endC), records[2]);
            }
        }


        [TestMethod]
        public void ReadAll_RecordsWithDateTimestampInQuotes()
        {
            var timestampConverter = TimestampStringConverterResolver.Default.Resolve("date");
            var context = new CsvSerializerContext() { TimestampConverter = timestampConverter };
            context.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetTypeConverter(timestampConverter));
            context.DataConverters.Add(new ValidationPointDataConverter());

            DateTimeOffset startTimeA = new DateTimeOffset(2018, 11, 20, 12, 08, 28, 477, TimeSpan.FromHours(2));
            DateTimeOffset endTimeA = startTimeA.AddSeconds(1);

            DateTimeOffset startTimeB = endTimeA.AddSeconds(2);
            DateTimeOffset endTimeB = startTimeB.AddSeconds(1);

            DateTimeOffset startTimeC = endTimeB.AddSeconds(2);
            DateTimeOffset endTimeC = startTimeC.AddSeconds(1);

            string[] lines = new[]
            {
                "Validation,Point,X,Y,StartTime,EndTime"
            ,  $"1,1,0.1,0.1,\"{context.TimestampConverter.ConvertBack(startTimeA)}\",\"{context.TimestampConverter.ConvertBack(endTimeA)}\""
            ,  $"2,1,0.5,0.9,\"{context.TimestampConverter.ConvertBack(startTimeB)}\",\"{context.TimestampConverter.ConvertBack(endTimeB)}\""
            ,  $"3,1,0.9,0.1,\"{context.TimestampConverter.ConvertBack(startTimeC)}\",\"{context.TimestampConverter.ConvertBack(endTimeC)}\""
            };

            string input = String.Join(Environment.NewLine, lines);

            using (var inputReader = new StringReader(input))
            using (var reader = new CsvDataReader(inputReader, typeof(ValidationPoint), context))
            {
                var records = reader.ReadAll<ValidationPoint>().ToList();

                Assert.AreEqual(3, records.Count);

                AssertRecord(new ValidationPoint(1, new GazeToolkit.Point2(0.1, 0.1), startTimeA, endTimeA), records[0]);
                AssertRecord(new ValidationPoint(2, new GazeToolkit.Point2(0.5, 0.9), startTimeB, endTimeB), records[1]);
                AssertRecord(new ValidationPoint(3, new GazeToolkit.Point2(0.9, 0.1), startTimeC, endTimeC), records[2]);
            }
        }


        [TestMethod]
        public void ReadAll_RecordsWithTicksTimestamp()
        {
            var timestampConverter = TimestampStringConverterResolver.Default.Resolve("ticks");
            var context = new CsvSerializerContext() { TimestampConverter = timestampConverter };
            context.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetTypeConverter(timestampConverter));
            context.DataConverters.Add(new ValidationPointDataConverter());

            long startTimeA = new DateTimeOffset(2018, 11, 20, 12, 08, 28, 477, TimeSpan.FromHours(2)).Ticks;
            long endTimeA = startTimeA + TimeSpan.FromSeconds(1).Ticks;

            long startTimeB = endTimeA + TimeSpan.FromSeconds(1).Ticks;
            long endTimeB = startTimeB + TimeSpan.FromSeconds(1).Ticks;

            long startTimeC = endTimeB + TimeSpan.FromSeconds(1).Ticks;
            long endTimeC = startTimeC + TimeSpan.FromSeconds(1).Ticks;

            string[] lines = new[]
            {
                "Validation,Point,X,Y,StartTime,EndTime"
            ,  $"1,1,0.1,0.1,{startTimeA},{endTimeA}"
            ,  $"2,1,0.5,0.9,{startTimeB},{endTimeB}"
            ,  $"3,1,0.9,0.1,{startTimeC},{endTimeC}"
            };

            string input = String.Join(Environment.NewLine, lines);

            using (var inputReader = new StringReader(input))
            using (var reader = new CsvDataReader(inputReader, typeof(ValidationPoint), context))
            {
                var records = reader.ReadAll<ValidationPoint>().ToList();

                Assert.AreEqual(3, records.Count);

                AssertRecord(new ValidationPoint(1, new GazeToolkit.Point2(0.1, 0.1), new DateTimeOffset(startTimeA, TimeSpan.Zero), new DateTimeOffset(endTimeA, TimeSpan.Zero)), records[0]);
                AssertRecord(new ValidationPoint(2, new GazeToolkit.Point2(0.5, 0.9), new DateTimeOffset(startTimeB, TimeSpan.Zero), new DateTimeOffset(endTimeB, TimeSpan.Zero)), records[1]);
                AssertRecord(new ValidationPoint(3, new GazeToolkit.Point2(0.9, 0.1), new DateTimeOffset(startTimeC, TimeSpan.Zero), new DateTimeOffset(endTimeC, TimeSpan.Zero)), records[2]);
            }
        }


        private void AssertRecord(ValidationPoint expected, ValidationPoint actual)
        {
            Assert.AreEqual(expected.Validation, actual.Validation);
            Assert.AreEqual(expected.Point, actual.Point);
            Assert.AreEqual(expected.StartTime, actual.StartTime);
            Assert.AreEqual(expected.EndTime, actual.EndTime);
        }


        [TestMethod]
        public void ReadAll_Empty_NoResult()
        {
            var timestampConverter = TimestampStringConverterResolver.Default.Resolve("date");
            var context = new CsvSerializerContext() { TimestampConverter = timestampConverter };
            context.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetTypeConverter(timestampConverter));
            context.DataConverters.Add(new ValidationPointDataConverter());

            string[] lines = new[]
            {
                "Validation,X,Y,StartTime,EndTime"
            };

            string input = String.Join(Environment.NewLine, lines);

            using (var inputReader = new StringReader(input))
            using (var reader = new CsvDataReader(inputReader, typeof(ValidationPoint), context))
            {
                var records = reader.ReadAll<ValidationPoint>().ToList();

                Assert.AreEqual(0, records.Count);
            }
        }


        [TestMethod]
        public void ReadAll_EmptyWithoutHeader_NoResult()
        {
            var timestampConverter = TimestampStringConverterResolver.Default.CreateDefaultConverter();
            var context = new CsvSerializerContext() { TimestampConverter = timestampConverter };
            context.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetTypeConverter(timestampConverter));
            context.DataConverters.Add(new ValidationPointDataConverter());

            string[] lines = new string[0];

            string input = String.Join(Environment.NewLine, lines);

            using (var inputReader = new StringReader(input))
            using (var reader = new CsvDataReader(inputReader, typeof(ValidationPoint), context))
            {
                var records = reader.ReadAll<ValidationPoint>().ToList();

                Assert.AreEqual(0, records.Count);
            }
        }

        
        [TestMethod]
        [ExpectedException(typeof(SerializationException))]
        public void ReadAll_MissingFieldInRecord_ExceptionThrown()
        {
            var timestampConverter = TimestampStringConverterResolver.Default.Resolve("ticks");
            var context = new CsvSerializerContext() { TimestampConverter = timestampConverter };
            context.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetTypeConverter(timestampConverter));
            context.DataConverters.Add(new ValidationPointDataConverter());

            long startTimeA = new DateTimeOffset(2018, 11, 20, 12, 08, 28, 477, TimeSpan.FromHours(2)).Ticks;
            long endTimeA = startTimeA + TimeSpan.FromSeconds(1).Ticks;

            long startTimeB = endTimeA + TimeSpan.FromSeconds(1).Ticks;
            // long endTimeB = startTimeB + TimeSpan.FromSeconds(1).Ticks;

            string[] lines = new[]
            {
                "Validation,Point,X,Y,StartTime,EndTime"
            ,  $"1,1,0.1,0.1,\"{startTimeA}\",\"{endTimeA}\""
            ,  $"1,2,0.5,0.9,\"{startTimeB}\","
            };

            string input = String.Join(Environment.NewLine, lines);

            using (var inputReader = new StringReader(input))
            using (var reader = new CsvDataReader(inputReader, typeof(ValidationPoint), context))
            {
                var records = reader.ReadAll<ValidationPoint>().ToList();
            }
        }
    }
}
