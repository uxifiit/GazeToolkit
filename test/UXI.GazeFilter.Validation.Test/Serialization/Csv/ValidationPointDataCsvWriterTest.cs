using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeFilter.Validation.Serialization.Csv.Converters;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Serialization;
using UXI.GazeToolkit.Serialization.Csv;
using UXI.GazeToolkit.Validation;
using UXI.Serialization.Formats.Csv;
using UXI.Serialization.Formats.Csv.Configurations;
using UXI.Filters.Serialization.Csv;
using UXI.Filters.Serialization.Converters;

namespace UXI.GazeFilter.Validation.Serialization.Csv
{
    [TestClass]
    public class ValidationPointDataCsvWriterTest
    {
        // Custom delimiter string for ending the lines in the string builders used for recording output.
        // If such value was contained in a data field, the CsvWriter would escape quotes and we would split the final output string at the right positions.
        private const string NewLineDelimiter = "<\"CR\">";

        private const string ExpectedHeaderLine = "Validation,Point,X,Y,RelativeTimestamp,Timestamp,LeftValidity,LeftGazePoint2DX,LeftGazePoint2DY,LeftGazePoint3DX,LeftGazePoint3DY,LeftGazePoint3DZ,LeftEyePosition3DX,LeftEyePosition3DY,LeftEyePosition3DZ,LeftPupilDiameter,RightValidity,RightGazePoint2DX,RightGazePoint2DY,RightGazePoint3DX,RightGazePoint3DY,RightGazePoint3DZ,RightEyePosition3DX,RightEyePosition3DY,RightEyePosition3DZ,RightPupilDiameter";
        private static DateTimeOffset startTime = DateTimeOffset.MinValue.Add(TimeSpan.FromMilliseconds(800));

        private static readonly string[] ExpectedSampleCsv = new[]
        {
             ExpectedHeaderLine
           , "1,1,0.1,0.5,0,8000000,Valid,0.11,0.58,-180.11,-152.8,580.14,-180.11,-152.8,0,5.14,Valid,0.14,0.65,-180.11,-152.8,604.2,-180.11,-152.8,0,5.14"
           , "1,1,0.1,0.5,16.666,8166660,Valid,0.13,0.51,-180.11,-152.8,550,-180.11,-152.8,0,5.14,Valid,0.12,0.56,-180.11,-152.8,634.5,-180.11,-152.8,0,5.14"
           , "1,2,0.5,0.9,16.666,28166660,Valid,0.11,0.58,-180.11,-152.8,580.14,-180.11,-152.8,0,5.14,Invalid,-0.1,-0.1,0,0,0,0,0,0,0"
           , "1,2,0.5,0.9,33.332,28333320,Invalid,-0.1,-0.1,0,0,0,0,0,0,0,Invalid,-0.1,-0.1,0,0,0,0,0,0,0"
        };

        private static readonly ValidationPointData[] SampleData = new[]
        {
            new ValidationPointData
            (
                new ValidationPoint(1, 1, new GazeToolkit.Point2(0.1, 0.5), startTime, startTime.AddSeconds(1)),
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
            new ValidationPointData
            (
                new ValidationPoint(1, 2, new GazeToolkit.Point2(0.5, 0.9), startTime.AddSeconds(2), startTime.AddSeconds(3)),
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
            var serialization = new CsvSerializationFactory
            (
                new CsvGazeToolkitDataConvertersSerializationConfiguration(),
                new CsvTimestampedDataSerializationConfiguration("Timestamp"),
                new CsvTimestampSerializationConfiguration(TimestampStringConverterResolver.Default.CreateDefaultConverter()),
                new CsvConvertersSerializationConfiguration(new ValidationPointDataCsvConverter())
            );

            IEnumerable<ValidationPointData> data = SampleData.Take(1).Select(p => new ValidationPointData(p.Point, p.Data.Take(1)));
            string[] expectedLines = ExpectedSampleCsv.Take(2).ToArray();

            StringBuilder sb = new StringBuilder();

            using (var output = new StringWriter(sb) { NewLine = NewLineDelimiter })
            using (var writer = serialization.CreateWriterForType(output, typeof(ValidationPointData), null))
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
            var serialization = new CsvSerializationFactory
            (
                new CsvGazeToolkitDataConvertersSerializationConfiguration(),
                new CsvTimestampedDataSerializationConfiguration("Timestamp"),
                new CsvTimestampSerializationConfiguration(TimestampStringConverterResolver.Default.CreateDefaultConverter()),
                new CsvConvertersSerializationConfiguration(new ValidationPointDataCsvConverter())
            );

            IEnumerable<ValidationPointData> data = SampleData.Take(1);
            string[] expectedLines = ExpectedSampleCsv.Take(3).ToArray();

            StringBuilder sb = new StringBuilder();

            using (var output = new StringWriter(sb) { NewLine = NewLineDelimiter })
            using (var writer = serialization.CreateWriterForType(output, typeof(ValidationPointData), null))
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
            var serialization = new CsvSerializationFactory
            (
                new CsvGazeToolkitDataConvertersSerializationConfiguration(),
                new CsvTimestampedDataSerializationConfiguration("Timestamp"),
                new CsvTimestampSerializationConfiguration(TimestampStringConverterResolver.Default.CreateDefaultConverter()),
                new CsvConvertersSerializationConfiguration(new ValidationPointDataCsvConverter())
            );

            IEnumerable<ValidationPointData> data = SampleData;
            string[] expectedLines = ExpectedSampleCsv.ToArray();

            StringBuilder sb = new StringBuilder();

            using (var output = new StringWriter(sb) { NewLine = NewLineDelimiter })
            using (var writer = serialization.CreateWriterForType(output, typeof(ValidationPointData), null))
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
            var serialization = new CsvSerializationFactory
            (
                new CsvGazeToolkitDataConvertersSerializationConfiguration(),
                new CsvTimestampedDataSerializationConfiguration("Timestamp"),
                new CsvTimestampSerializationConfiguration(TimestampStringConverterResolver.Default.CreateDefaultConverter()),
                new CsvConvertersSerializationConfiguration(new ValidationPointDataCsvConverter())
            );

            IEnumerable<ValidationPointData> data = Enumerable.Empty<ValidationPointData>();
            string[] expectedLines = ExpectedSampleCsv.Take(1).ToArray();

            StringBuilder sb = new StringBuilder();

            using (var output = new StringWriter(sb) { NewLine = NewLineDelimiter })
            using (var writer = serialization.CreateWriterForType(output, typeof(ValidationPointData), null))
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
