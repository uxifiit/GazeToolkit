using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UXI.GazeToolkit.Serialization.Json.Converters
{
    public abstract class JsonConverter<T> : Newtonsoft.Json.JsonConverter
    {
        private readonly bool _canBeNull = (typeof(T).IsValueType == false) || (Nullable.GetUnderlyingType(typeof(T)) != null);

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(T));
        }


        protected abstract T Convert(JToken token, JsonSerializer serializer);


        public sealed override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null && _canBeNull)
            {
                return null;
            }
            else
            {
                // Load the JSON for the Result into a JObject
                JToken token = JToken.Load(reader);

                // Construct the Result object using the conversion function
                T result = Convert(token, serializer);

                // Return the result
                return result;
            }
        }


        public override bool CanWrite
        {
            get { return false; }
        }


        protected virtual JToken ConvertBack(T value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }


        public sealed override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken token = ConvertBack((T)value, serializer);

            token.WriteTo(writer);
        }
    }
}
