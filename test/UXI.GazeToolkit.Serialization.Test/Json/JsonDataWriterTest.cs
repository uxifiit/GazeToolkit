using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Serialization.Formats.Json;

namespace UXI.GazeToolkit.Serialization.Json
{
    [TestClass]
    public class JsonDataWriterTest
    {
        [TestMethod]
        public void Write_TwoObjects_JsonArrayWithTwoObjects()
        {
            string expected = "[{},{}]";

            var serializer = new JsonSerializer();

            using (var sw = new StringWriter())
            {
                using (var writer = new JsonDataWriter(sw, serializer))
                {
                    writer.Write(new object());
                    writer.Write(new object());
                }

                Assert.AreEqual(expected, sw.ToString());
            }
        }


        [TestMethod]
        public void Write_MultipleObjects_JsonArray()
        {
            string expected = "[{},{},{},{}]";

            var serializer = new JsonSerializer();

            using (var sw = new StringWriter())
            {
                using (var writer = new JsonDataWriter(sw, serializer))
                {
                    writer.Write(new object());
                    writer.Write(new object());
                    writer.Write(new object());
                    writer.Write(new object());
                }

                Assert.AreEqual(expected, sw.ToString());
            }
        }


        [TestMethod]
        public void Write_SingleEmptyObjectWithoutExplicitClose_JsonOfSingleObject()
        {
            string expected = "{}";

            var serializer = new JsonSerializer();

            using (var sw = new StringWriter())
            {
                using (var writer = new JsonDataWriter(sw, serializer))
                {
                    writer.Write(new object());
                }

                Assert.AreEqual(expected, sw.ToString());
            }
        }


        [TestMethod]
        public void Close_ExplicitAfterWritingSingleObject_JsonOfSingleObject()
        {
            string expected = "{}";

            var serializer = new JsonSerializer();

            using (var sw = new StringWriter())
            using (var writer = new JsonDataWriter(sw, serializer))
            {
                writer.Write(new object());

                writer.Close();
                
                Assert.AreEqual(expected, sw.ToString());
            }
        }


        [TestMethod]
        public void Dispose_AfterWritingSingleObject_JsonOfSingleObject()
        {
            string expected = "{}";

            var serializer = new JsonSerializer();

            using (var sw = new StringWriter())
            {
                using (var writer = new JsonDataWriter(sw, serializer))
                {
                    writer.Write(new object());
                }

                Assert.AreEqual(expected, sw.ToString());
            }
        }
    }
}
