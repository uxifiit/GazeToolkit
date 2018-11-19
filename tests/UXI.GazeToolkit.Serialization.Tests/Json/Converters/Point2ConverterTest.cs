using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using UXI.GazeToolkit.Serialization.Json.Converters;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit.Serialization.Tests.Json.Converters
{
    [TestClass]
    public class Point2ConverterTest
    {
        JsonSerializer serializer;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new JsonSerializer();
            serializer.Converters.Add(new Point2Converter());
        }


        [TestMethod]
        public void TestConvert_FromArray()
        {
            var expected = new Point2(0.2, 0.3);
            string json = "[0.2, 0.3]";

            using (var sr = new StringReader(json))
            {
                var point = serializer.Deserialize(sr, typeof(Point2));

                Assert.IsNotNull(point);
                Assert.AreEqual(expected, point);
            }
        }


        [TestMethod]
        public void TestConvert_EmptyObject()
        {
            var expected = new Point2();
            string json = "{}";

            using (var sr = new StringReader(json))
            {
                var point = serializer.Deserialize(sr, typeof(Point2));

                Assert.AreEqual(expected, point);
            }
        }


        [TestMethod]
        public void TestConvert_FromObject()
        {
            var expected = new Point2(0.2, 0.3);
            string json = "{ \"x\": 0.2, \"y\": 0.3 }";

            using (var sr = new StringReader(json))
            {
                var point = serializer.Deserialize(sr, typeof(Point2));

                Assert.AreEqual(expected, point);
            }
        }


        [TestMethod]
        public void TestConvert_Null()
        {
            var expected = new Point2();
            string json = "null";

            using (var sr = new StringReader(json))
            {
                var point = serializer.Deserialize(sr, typeof(Point2));

                Assert.AreEqual(expected, point);
            }
        }


        [TestMethod]
        public void TestConvert_NullableNull()
        {
            Point2? expected = null;
            string json = "null";

            using (var sr = new StringReader(json))
            {
                var point = serializer.Deserialize(sr, typeof(Point2?));

                Assert.AreEqual(expected, point);
            }
        }
    }
}
