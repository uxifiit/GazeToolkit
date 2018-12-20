using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Serialization.Formats.Csv.Converters;
using UXI.GazeToolkit.Serialization.Json.Converters;
using UXI.Serialization;
using UXI.Serialization.Json;
using UXI.Serialization.Json.Converters;

namespace UXI.GazeToolkit.Serialization.Json
{
    public class JsonDataConvertersSerializationConfiguration : JsonConvertersSerializationConfiguration
    {
        public JsonDataConvertersSerializationConfiguration()
            : base
            (
                new Point2Converter(),
                new Point3JsonConverter(),
                new EyeSampleJsonConverter(),
                new EyeDataJsonConverter(),
                new GazeDataJsonConverter(),
                new SingleEyeGazeDataJsonConverter(),
                new EyeVelocityJsonConverter(),
                new EyeMovementJsonConverter()
            )
        { }
    }
}
