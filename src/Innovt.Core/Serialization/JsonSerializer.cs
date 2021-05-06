// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Innovt.Core.Serialization
{
    public class JsonSerializer : ISerializer
    {
        private readonly JsonSerializerOptions options;

        public JsonSerializer(bool ignoreNullValues = true, bool ignoreReadOnlyProperties = true,
            bool propertyNameCaseInsensitive = true, List<JsonConverter> converters = null)
        {
            options = new JsonSerializerOptions
            {
                IgnoreNullValues = ignoreNullValues,
                IgnoreReadOnlyProperties = ignoreReadOnlyProperties,
                PropertyNameCaseInsensitive = propertyNameCaseInsensitive
            };

            converters?.ForEach(c => options.Converters.Add(c));
        }

        public T DeserializeObject<T>(string serializedObject)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(serializedObject, options);
        }

        public string SerializeObject<T>(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return System.Text.Json.JsonSerializer.Serialize(obj, options);
        }
    }
}