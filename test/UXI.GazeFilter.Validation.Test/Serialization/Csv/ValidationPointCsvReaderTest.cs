using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using UXI.GazeToolkit.Validation;
using System.Linq;
using System.Runtime.Serialization;
using UXI.GazeFilter.Validation.Serialization.Csv.Converters;
using UXI.GazeToolkit.Serialization.Csv;
using UXI.Serialization.Formats.Csv;
using UXI.Serialization.Extensions;
using UXI.GazeToolkit.Serialization;
using UXI.Serialization.Formats.Csv.Configurations;
using UXI.Serialization.Configurations;
using UXI.Filters.Serialization.Csv;
using UXI.Filters.Serialization.Converters;

namespace UXI.GazeFilter.Validation.Serialization.Csv
{
    /// <summary>
    /// Summary description for ValidationPointCsvReaderTest
    /// </summary>
    [TestClass]
    public class ValidationPointCsvReaderTest
    {
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
            ITimestampStringConverter timestampConverter = TimestampStringConverterResolver.Default.Resolve("date");

            var serialization = new CsvSerializationFactory
            (
                new CsvGazeToolkitDataConvertersSerializationConfiguration(),
                new CsvTimestampedDataSerializationConfiguration("Timestamp"),
                new CsvTimestampSerializationConfiguration(timestampConverter),
                new CsvConvertersSerializationConfiguration(new ValidationPointCsvConverter())
            );

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
            ,  $"1,1,0.1,0.1,{timestampConverter.ConvertBack(startA)},{timestampConverter.ConvertBack(endA)}"
            ,  $"2,1,0.5,0.9,{timestampConverter.ConvertBack(startB)},{timestampConverter.ConvertBack(endB)}"
            ,  $"3,1,0.9,0.1,{timestampConverter.ConvertBack(startC)},{timestampConverter.ConvertBack(endC)}"
            };

            string input = String.Join(Environment.NewLine, lines);

            using (var inputReader = new StringReader(input))
            using (var reader = serialization.CreateReaderForType(inputReader, typeof(ValidationPoint), null))
            {
                var records = reader.ReadAll<ValidationPoint>().ToList();

                Assert.AreEqual(3, records.Count);

                AssertRecord(new ValidationPoint(1, 1, new GazeToolkit.Point2(0.1, 0.1), startA, endA), records[0]);
                AssertRecord(new ValidationPoint(2, 1, new GazeToolkit.Point2(0.5, 0.9), startB, endB), records[1]);
                AssertRecord(new ValidationPoint(3, 1, new GazeToolkit.Point2(0.9, 0.1), startC, endC), records[2]);
            }
        }


        [TestMethod]
        public void ReadAll_RecordsWithDateTimestampInQuotes()
        {
            ITimestampStringConverter timestampConverter = TimestampStringConverterResolver.Default.Resolve("date");

            var serialization = new CsvSerializationFactory
            (
                new CsvGazeToolkitDataConvertersSerializationConfiguration(),
                new CsvTimestampedDataSerializationConfiguration("Timestamp"),
                new CsvTimestampSerializationConfiguration(timestampConverter),
                new CsvConvertersSerializationConfiguration(new ValidationPointCsvConverter())
            );

            DateTimeOffset startTimeA = new DateTimeOffset(2018, 11, 20, 12, 08, 28, 477, TimeSpan.FromHours(2));
            DateTimeOffset endTimeA = startTimeA.AddSeconds(1);

            DateTimeOffset startTimeB = endTimeA.AddSeconds(2);
            DateTimeOffset endTimeB = startTimeB.AddSeconds(1);

            DateTimeOffset startTimeC = endTimeB.AddSeconds(2);
            DateTimeOffset endTimeC = startTimeC.AddSeconds(1);

            string[] lines = new[]
            {
                "Validation,Point,X,Y,StartTime,EndTime"
            ,  $"1,1,0.1,0.1,\"{timestampConverter.ConvertBack(startTimeA)}\",\"{timestampConverter.ConvertBack(endTimeA)}\""
            ,  $"2,1,0.5,0.9,\"{timestampConverter.ConvertBack(startTimeB)}\",\"{timestampConverter.ConvertBack(endTimeB)}\""
            ,  $"3,1,0.9,0.1,\"{timestampConverter.ConvertBack(startTimeC)}\",\"{timestampConverter.ConvertBack(endTimeC)}\""
            };

            string input = String.Join(Environment.NewLine, lines);

            using (var inputReader = new StringReader(input))
            using (var reader = serialization.CreateReaderForType(inputReader, typeof(ValidationPoint), null))
            {
                var records = reader.ReadAll<ValidationPoint>().ToList();

                Assert.AreEqual(3, records.Count);

                AssertRecord(new ValidationPoint(1, 1, new GazeToolkit.Point2(0.1, 0.1), startTimeA, endTimeA), records[0]);
                AssertRecord(new ValidationPoint(2, 1, new GazeToolkit.Point2(0.5, 0.9), startTimeB, endTimeB), records[1]);
                AssertRecord(new ValidationPoint(3, 1, new GazeToolkit.Point2(0.9, 0.1), startTimeC, endTimeC), records[2]);
            }
        }


        [TestMethod]
        public void ReadAll_RecordsWithTicksTimestamp()
        {
            var serialization = new CsvSerializationFactory
            (
                new CsvGazeToolkitDataConvertersSerializationConfiguration(),
                new CsvTimestampedDataSerializationConfiguration("Timestamp"),
                new CsvTimestampSerializationConfiguration(TimestampStringConverterResolver.Default.Resolve("ticks")),
                new CsvConvertersSerializationConfiguration(new ValidationPointCsvConverter())
            );

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
            using (var reader = serialization.CreateReaderForType(inputReader, typeof(ValidationPoint), null))
            {
                var records = reader.ReadAll<ValidationPoint>().ToList();

                Assert.AreEqual(3, records.Count);

                AssertRecord(new ValidationPoint(1, 1, new GazeToolkit.Point2(0.1, 0.1), new DateTimeOffset(startTimeA, TimeSpan.Zero), new DateTimeOffset(endTimeA, TimeSpan.Zero)), records[0]);
                AssertRecord(new ValidationPoint(2, 1, new GazeToolkit.Point2(0.5, 0.9), new DateTimeOffset(startTimeB, TimeSpan.Zero), new DateTimeOffset(endTimeB, TimeSpan.Zero)), records[1]);
                AssertRecord(new ValidationPoint(3, 1, new GazeToolkit.Point2(0.9, 0.1), new DateTimeOffset(startTimeC, TimeSpan.Zero), new DateTimeOffset(endTimeC, TimeSpan.Zero)), records[2]);
            }
        }


        private void AssertRecord(ValidationPoint expected, ValidationPoint actual)
        {
            Assert.AreEqual(expected.Validation, actual.Validation);
            Assert.AreEqual(expected.Point, actual.Point);
            Assert.AreEqual(expected.Position, actual.Position);
            Assert.AreEqual(expected.StartTime, actual.StartTime);
            Assert.AreEqual(expected.EndTime, actual.EndTime);
        }


        [TestMethod]
        public void ReadAll_Empty_NoResult()
        {
            var serialization = new CsvSerializationFactory
            (
                new CsvGazeToolkitDataConvertersSerializationConfiguration(),
                new CsvTimestampedDataSerializationConfiguration("Timestamp"),
                new CsvTimestampSerializationConfiguration(TimestampStringConverterResolver.Default.Resolve("date")),
                new CsvConvertersSerializationConfiguration(new ValidationPointCsvConverter())
            );

            string[] lines = new[]
            {
                "Validation,X,Y,StartTime,EndTime"
            };

            string input = String.Join(Environment.NewLine, lines);

            using (var inputReader = new StringReader(input))
            using (var reader = serialization.CreateReaderForType(inputReader, typeof(ValidationPoint), null))
            {
                var records = reader.ReadAll<ValidationPoint>().ToList();

                Assert.AreEqual(0, records.Count);
            }
        }


        [TestMethod]
        public void ReadAll_EmptyWithoutHeader_NoResult()
        {
            var serialization = new CsvSerializationFactory
            (
                new CsvGazeToolkitDataConvertersSerializationConfiguration(),
                new CsvTimestampedDataSerializationConfiguration("Timestamp"),
                new CsvTimestampSerializationConfiguration(TimestampStringConverterResolver.Default.Resolve("date")),
                new CsvConvertersSerializationConfiguration(new ValidationPointCsvConverter())
            );

            string[] lines = new string[0];

            string input = String.Join(Environment.NewLine, lines);

            using (var inputReader = new StringReader(input))
            using (var reader = serialization.CreateReaderForType(inputReader, typeof(ValidationPoint), null))
            {
                var records = reader.ReadAll<ValidationPoint>().ToList();

                Assert.AreEqual(0, records.Count);
            }
        }

        //// TODO Distinguish required fields in CSV serialization for decision whether throw exception or not.
        //// Current solution is not sufficient for the case, when member of a class (nullable type) cannot be deserialized.
        [TestMethod]
        [ExpectedException(typeof(SerializationException))]
        public void ReadAll_MissingFieldInRecord_ThrowsException()
        {
            var serialization = new CsvSerializationFactory
            (
                new RelaySerializationConfiguration<CsvSerializerContext>
                (
                    (serializer, _, __) => { serializer.ThrowOnFailedDeserialize = true; return serializer; }
                ),
                new CsvGazeToolkitDataConvertersSerializationConfiguration(),
                new CsvTimestampedDataSerializationConfiguration("Timestamp"),
                new CsvTimestampSerializationConfiguration(TimestampStringConverterResolver.Default.Resolve("ticks")),
                new CsvConvertersSerializationConfiguration(new ValidationPointCsvConverter())
            );

            long startTimeA = new DateTimeOffset(2018, 11, 20, 12, 08, 28, 477, TimeSpan.FromHours(2)).Ticks;
            long endTimeA = startTimeA + TimeSpan.FromSeconds(1).Ticks;

            long startTimeB = endTimeA + TimeSpan.FromSeconds(1).Ticks;
            // endTimeB is the missing field

            string[] lines = new[]
            {
                "Validation,Point,X,Y,StartTime,EndTime"
            ,  $"1,1,0.1,0.1,\"{startTimeA}\",\"{endTimeA}\""
            ,  $"1,2,0.5,0.9,\"{startTimeB}\","
            };

            string input = String.Join(Environment.NewLine, lines);

            using (var inputReader = new StringReader(input))
            using (var reader = serialization.CreateReaderForType(inputReader, typeof(ValidationPoint), null))
            {
                var records = reader.ReadAll<ValidationPoint>().ToList();
            }
        }
    }
}
