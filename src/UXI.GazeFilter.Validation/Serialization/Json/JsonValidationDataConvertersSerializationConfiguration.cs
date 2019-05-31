using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeFilter.Validation.Serialization.Json.Converter;
using UXI.GazeToolkit.Serialization.Json.Converters;
using UXI.Serialization;
using UXI.Serialization.Formats.Json;
using UXI.Serialization.Formats.Json.Configurations;
using UXI.Serialization.Formats.Json.Converters;

namespace UXI.GazeToolkit.Validation.Serialization.Json
{
    public class JsonValidationDataConvertersSerializationConfiguration : JsonConvertersSerializationConfiguration
    {
        public JsonValidationDataConvertersSerializationConfiguration()
            : base
            (
                new ValidationPointJsonConverter(),
                new DisplayAreaJsonConverter(),
                new DisplayAreaChangedEventJsonConverter()
            )
        { 
        }
    }
}
