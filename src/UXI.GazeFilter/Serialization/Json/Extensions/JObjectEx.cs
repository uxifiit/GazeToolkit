using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UXI.GazeFilter.Serialization.Json.Extensions
{
    internal static class JObjectEx
    {
        /// <summary>
        /// Gets the <see cref="JToken"/> with the specified property name and case ignoring. If the value is found, it is then converted into object of the requested type using the specified <see cref="JsonSerializer"/>.
        /// </summary>
        /// <typeparam name="TValue">Type which the <see cref="JToken"/> is converted into.</typeparam>
        /// <param name="jObject"></param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="serializer">The JsonSerializer that will be used when creating the object.</param>
        /// <returns></returns>
        public static TValue GetValue<TValue>(this JObject jObject, string propertyName, JsonSerializer serializer)
        {
            return jObject.GetValue(propertyName, StringComparison.CurrentCultureIgnoreCase).ToObject<TValue>(serializer);
        }


        /// <summary>
        /// Gets the <see cref="JToken"/> with the specified property name and case ignoring. If the value is found, it is then converted into object of the requested type using the specified <see cref="JsonSerializer"/>. If the value is not found, it returns default value for the requested type.
        /// </summary>
        /// <typeparam name="TValue">Type which the <see cref="JToken"/> is converted into.</typeparam>
        /// <param name="jObject"></param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="serializer">The JsonSerializer that will be used when creating the object.</param>
        /// <returns>Value of type TValue, if the object contains it; otherwise, default value of type TValue.</returns>
        public static TValue GetValueOrDefault<TValue>(this JObject jObject, string propertyName, JsonSerializer serializer)
        {
            JToken value;
            if (jObject.TryGetValue(propertyName, StringComparison.CurrentCultureIgnoreCase, out value))
            {
                return value.ToObject<TValue>(serializer);
            }

            return default(TValue);
        }
    }
}
