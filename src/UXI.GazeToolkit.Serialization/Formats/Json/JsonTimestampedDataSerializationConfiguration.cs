using Newtonsoft.Json;
using System;
using UXI.GazeToolkit.Serialization.Json.Converters;
using UXI.Serialization;
using UXI.Serialization.Configurations;

namespace UXI.GazeToolkit.Serialization.Json
{
    public class JsonTimestampedDataSerializationConfiguration : SerializationConfiguration<JsonSerializer>
    {
        public JsonTimestampedDataSerializationConfiguration(string fieldName)
        {
            FieldName = fieldName;
        }


        public string FieldName { get; set; }


        protected override JsonSerializer Configure(JsonSerializer serializer, DataAccess acess, object settings)
        {
            SetupTimestampedDataSerialization(serializer, FieldName);

            return serializer;
        }


        private void SetupTimestampedDataSerialization(JsonSerializer serializer, string timestampFieldName)
        {
            JsonConverter converter = String.IsNullOrWhiteSpace(timestampFieldName)
                                    ? new TimestampedDataJsonConverter()
                                    : new TimestampedDataJsonConverter(timestampFieldName);

            serializer.Converters.Add(converter);
        }
    }
}
