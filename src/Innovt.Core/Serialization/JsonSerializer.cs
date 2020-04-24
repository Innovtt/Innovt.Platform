using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Innovt.Core.Serialization
{
    public class JsonSerializer:ISerializer
    { 
        private readonly JsonSerializerOptions options = null;

        public JsonSerializer(bool ignoreNullValues=true, bool ignoreReadOnlyProperties=true,bool propertyNameCaseInsensitive=true,List<JsonConverter> converters=null)
        {
            options = new JsonSerializerOptions()
            {
                IgnoreNullValues = ignoreNullValues,
                IgnoreReadOnlyProperties = ignoreReadOnlyProperties,
                PropertyNameCaseInsensitive = propertyNameCaseInsensitive
            };

            converters?.ForEach(c=>options.Converters.Add(c));
        }

        public T DeserializeObject<T>(string serializedObject)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(serializedObject, options);
        }

        public string SerializeObject<T>(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return System.Text.Json.JsonSerializer.Serialize<T>(obj,options);
        }
    }
}