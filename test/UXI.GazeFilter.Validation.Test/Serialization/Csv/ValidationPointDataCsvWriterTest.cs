using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeFilter.Validation.Serialization.Csv.DataConverters;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Serialization.Converters;
using UXI.GazeToolkit.Serialization.Csv;
using UXI.GazeToolkit.Serialization.Csv.TypeConverters;
using UXI.GazeToolkit.Validation;

namespace UXI.GazeFilter.Validation.Serialization.Csv
{
    [TestClass]
    public class ValidationPointDataCsvWriterTest
    {
        // Custom delimiter string for ending the lines in the string builders used for recording output.
        // If such value was contained in a data field, the CsvWriter would escape quotes and we would split the final output string at the right positions.
        private const string NewLineDelimiter = "<\"CR\">";

        private const string ExpectedHeaderLine = "Validation,Point,X,Y,Timestamp,LeftValidity,LeftGazePointX,LeftGazePointY,LeftDistance,RightValidity,RightGazePointX,RightGazePointY,RightDistance";
        private static DateTimeOffset startTime = DateTimeOffset.MinValue.Add(TimeSpan.FromMilliseconds(800));
                     
        private static readonly ValidationPointGaze[] SampleData = new[]
        {
            new ValidationPointGaze
            (
                new ValidationPoint(1, new GazeToolkit.Point2(0.1, 0.5), startTime, startTime.AddSeconds(1)),
                new GazeData[]
                {
                    new GazeData
                    (               
                        new EyeData(EyeValidity.Valid, new Point2(0.11, 0.58), new Point3(-180.11, -152.8, 580.14), new Point3(-180.11, -152.8, 0), 5.14),
                        new EyeData(EyeValidity.Valid, new Point2(0.14, 0.65), new Point3(-180.11, -152.8, 604.2),  new Point3(-180.11, -152.8, 0), 5.14),
                        startTime
                    ),
                    new GazeData
                    (
                        new EyeData(EyeValidity.Valid, new Point2(0.13, 0.51), new Point3(-180.11, -152.8, 550),   new Point3(-180.11, -152.8, 0), 5.14),
                        new EyeData(EyeValidity.Valid, new Point2(0.12, 0.56), new Point3(-180.11, -152.8, 634.5), new Point3(-180.11, -152.8, 0), 5.14),
                        startTime.AddTicks(166660)
                    )
                }
            ),
            new ValidationPointGaze
            (
                new ValidationPoint(1, new GazeToolkit.Point2(0.5, 0.9), startTime.AddSeconds(2), startTime.AddSeconds(3)),
                new GazeData[]
                {
                    new GazeData
                    (
                        new EyeData(EyeValidity.Valid, new Point2(0.11, 0.58), new Point3(-180.11, -152.8, 580.14), new Point3(-180.11, -152.8, 0), 5.14),
                        new EyeData(EyeValidity.Invalid, new Point2(-0.1, -0.1), Point3.Zero, Point3.Zero, 0d),
                        startTime.AddSeconds(2).AddTicks(166660)
                    ),
                    new GazeData
                    (
                        new EyeData(EyeValidity.Invalid, new Point2(-0.1, -0.1), Point3.Zero, Point3.Zero, 0d),
                        new EyeData(EyeValidity.Invalid, new Point2(-0.1, -0.1), Point3.Zero, Point3.Zero, 0d),
                        startTime.AddSeconds(2).AddTicks(2 * 166660)
                    )
                }
            )
        };

        private static readonly string[] ExpectedSampleCsv = new[]
        {
             ExpectedHeaderLine
           , "1,1,0.1,0.5,0,Valid,0.11,0.58,580.14,Valid,0.14,0.65,604.2"
           , "1,1,0.1,0.5,16.666,Valid,0.13,0.51,550,Valid,0.12,0.56,634.5"
           , "1,2,0.5,0.9,16.666,Valid,0.11,0.58,580.14,Invalid,-0.1,-0.1,0"
           , "1,2,0.5,0.9,33.332,Invalid,-0.1,-0.1,0,Invalid,-0.1,-0.1,0"
        };


        private string[] SplitOutput(StringBuilder stringBuilder)
        {
            return stringBuilder.ToString().Split(new string[] { NewLineDelimiter }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        }


        private void AssertLines(string[] expectedLines, string[] actualLines)
        {
            Assert.AreEqual(expectedLines.Length, actualLines.Length);
            foreach (var pair in expectedLines.Zip(actualLines, (a, b) => new[] { a, b }))
            {
                Assert.AreEqual(pair[0], pair[1]);
            }
        }


        [TestMethod]
        public void Write_SinglePointSingleData()
        {
            var timestampConverter = TimestampStringConverterResolver.Default.CreateDefaultConverter();
            var context = new CsvSerializerContext() { TimestampConverter = timestampConverter };
            context.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetTypeConverter(timestampConverter));
            context.DataConverters.Add(new ValidationPointGazeDataConverter());

            IEnumerable<ValidationPointGaze> data = SampleData.Take(1).Select(p => new ValidationPointGaze(p.Point, p.Gaze.Take(1)));
            string[] expectedLines = ExpectedSampleCsv.Take(2).ToArray();

            StringBuilder sb = new StringBuilder();

            using (var output = new StringWriter(sb) { NewLine = NewLineDelimiter })
            using (var writer = new CsvDataWriter(output, typeof(ValidationPointGaze), context))
            {
                foreach (var point in data)
                {
                    writer.Write(point);
                }
                writer.Close();
            }

            string[] actualLines = SplitOutput(sb);

            AssertLines(expectedLines, actualLines);
        }


        [TestMethod]
        public void Write_SinglePointMultipleData()
        {
            var timestampConverter = TimestampStringConverterResolver.Default.CreateDefaultConverter();
            var context = new CsvSerializerContext() { TimestampConverter = timestampConverter };
            context.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetTypeConverter(timestampConverter));
            context.DataConverters.Add(new ValidationPointGazeDataConverter());

            IEnumerable<ValidationPointGaze> data = SampleData.Take(1);
            string[] expectedLines = ExpectedSampleCsv.Take(3).ToArray();

            StringBuilder sb = new StringBuilder();

            using (var output = new StringWriter(sb) { NewLine = NewLineDelimiter })
            using (var writer = new CsvDataWriter(output, typeof(ValidationPointGaze), context))
            {
                foreach (var point in data)
                {
                    writer.Write(point);
                }
                writer.Close();
            }

            string[] actualLines = SplitOutput(sb);

            AssertLines(expectedLines, actualLines);
        }


        [TestMethod]
        public void Write_MultiplePointsMultipleData()
        {
            var timestampConverter = TimestampStringConverterResolver.Default.CreateDefaultConverter();
            var context = new CsvSerializerContext() { TimestampConverter = timestampConverter };
            context.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetTypeConverter(timestampConverter));
            context.DataConverters.Add(new ValidationPointGazeDataConverter());

            IEnumerable<ValidationPointGaze> data = SampleData;
            string[] expectedLines = ExpectedSampleCsv.ToArray();

            StringBuilder sb = new StringBuilder();

            using (var output = new StringWriter(sb) { NewLine = NewLineDelimiter })
            using (var writer = new CsvDataWriter(output, typeof(ValidationPointGaze), context))
            {
                foreach (var point in data)
                {
                    writer.Write(point);
                }
                writer.Close();
            }

            string[] actualLines = SplitOutput(sb);

            AssertLines(expectedLines, actualLines);
        }


        [TestMethod]
        public void Write_NoRecord()
        {
            var timestampConverter = TimestampStringConverterResolver.Default.CreateDefaultConverter();
            var context = new CsvSerializerContext() { TimestampConverter = timestampConverter };
            context.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetTypeConverter(timestampConverter));
            context.DataConverters.Add(new ValidationPointGazeDataConverter());

            IEnumerable<ValidationPointGaze> data = Enumerable.Empty<ValidationPointGaze>();
            string[] expectedLines = ExpectedSampleCsv.Take(1).ToArray();

            StringBuilder sb = new StringBuilder();

            using (var output = new StringWriter(sb) { NewLine = NewLineDelimiter })
            using (var writer = new CsvDataWriter(output, typeof(ValidationPointGaze), context))
            {
                foreach (var point in data)
                {
                    writer.Write(point);
                }
                writer.Close();
            }

            string[] actualLines = SplitOutput(sb);

            AssertLines(expectedLines, actualLines);
        }
    }
}
