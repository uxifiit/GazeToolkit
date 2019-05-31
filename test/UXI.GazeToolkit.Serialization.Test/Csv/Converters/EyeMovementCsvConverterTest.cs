using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UXI.GazeToolkit.Serialization.Csv.TypeConverters;
using UXI.Serialization.Configurations;
using UXI.Serialization.Formats.Csv;
using UXI.Serialization.Formats.Csv.Configurations;
using UXI.Serialization.Extensions;

namespace UXI.GazeToolkit.Serialization.Csv.Converters
{
    [TestClass]
    public class EyeMovementCsvConverterTest
    {
        [TestMethod]
        public void WriteCsv_FixationMovementWithNoSamples_RecordWithNull()
        {
            var factory = new CsvSerializationFactory
            (
                new CsvTimestampedDataSerializationConfiguration("Timestamp"),
                new RelaySerializationConfiguration<CsvSerializerContext>((serializer, access, settings) =>
                {
                    serializer.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetFromTicksTypeConverter());
                    return serializer;
                }),
                new CsvConvertersSerializationConfiguration
                (
                    new EyeMovementCsvConverter(), 
                    new TimestampedDataCsvConverter(), 
                    new EyeSampleCsvConverter(),
                    new Point2CsvConverter(), 
                    new Point3CsvConverter()
                )
            );

            string[] output = null;

            using (var result = new StringWriter())
            using (var writer = factory.CreateWriterForType(result, typeof(EyeMovement), null))
            {
                writer.Write(new EyeMovement(EyeMovementType.Fixation, null, new DateTimeOffset(636818976000000000, TimeSpan.Zero), new DateTimeOffset(636818976001200000, TimeSpan.Zero)));

                output = result.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            }

            string[] expected = new[]
            {
                "Timestamp,MovementType,Duration,AverageGazePoint2DX,AverageGazePoint2DY,AverageGazePoint3DX,AverageGazePoint3DY,AverageGazePoint3DZ,AverageEyePosition3DX,AverageEyePosition3DY,AverageEyePosition3DZ,AveragePupilDiameter"
            ,   "636818976000000000,Fixation,120,,,,,,,,,"
            ,   ""
            };

            Assert.AreEqual(expected.Length, output.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], output[i], $"Elements do not match at index {i}.");
            }
        }


        [TestMethod]
        public void ReadCsv_FixationMovementWithNoSamples_RecordWithNoAverage()
        {
            var factory = new CsvSerializationFactory
            (
                new CsvTimestampedDataSerializationConfiguration("Timestamp"),
                
                new RelaySerializationConfiguration<CsvSerializerContext>((serializer, access, settings) =>
                {
                    serializer.Configuration.TypeConverterCache.AddConverter<DateTimeOffset>(new DateTimeOffsetFromTicksTypeConverter());
                    return serializer;
                }),
                new CsvConvertersSerializationConfiguration
                (
                    new EyeMovementCsvConverter(),
                    new TimestampedDataCsvConverter(),
                    new EyeSampleCsvConverter() { ThrowOnFailedRead = false },
                    new Point2CsvConverter(),
                    new Point3CsvConverter()
                )
            );

            string[] csv = new[]
            {
                "Timestamp,MovementType,Duration,AverageGazePoint2DX,AverageGazePoint2DY,AverageGazePoint3DX,AverageGazePoint3DY,AverageGazePoint3DZ,AverageEyePosition3DX,AverageEyePosition3DY,AverageEyePosition3DZ,AveragePupilDiameter"
            ,   "636818976000000000,Fixation,120,,,,,,,,,"
            ,   ""
            };


            List<EyeMovement> movements = null;

            using (var source = new StringReader(String.Join(Environment.NewLine, csv)))
            using (var reader = factory.CreateReaderForType(source, typeof(EyeMovement), null))
            {
                movements = reader.ReadAll<EyeMovement>()?.ToList();
            }

            Assert.IsNotNull(movements);
            Assert.AreEqual(1, movements.Count);
            Assert.AreEqual(636818976000000000, movements[0].Timestamp.Ticks);
            Assert.AreEqual(EyeMovementType.Fixation, movements[0].MovementType);
        }
    }
}
