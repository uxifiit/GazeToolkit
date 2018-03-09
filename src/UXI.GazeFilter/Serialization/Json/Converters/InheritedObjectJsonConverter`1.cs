using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UXI.Data.Serialization.Json.Converters
{
    public abstract class InheritedObjectJsonConverter<T> : JsonConverter
    {
        protected abstract Type ResolveType(JObject jObject);

        protected T Create(Type objectType, JObject jObject)
        {
            Type type = ResolveType(jObject);

            if (type == null)
            {
                throw new Exception(String.Format("The given type is not supported!"));
            }
            else
            {
                return (T)Activator.CreateInstance(type);
            }
        }


        public override bool CanConvert(Type objectType)
        {
            return typeof(T).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target;
            try
            {
                target = Create(objectType, jObject);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Failed to deserialize JSON");
                throw; 
            }

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override bool CanRead => true;
    }
}
