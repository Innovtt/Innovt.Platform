﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Innovt.Core.Serialization;

public class JsonSerializer : ISerializer
{
    private readonly JsonSerializerOptions options;

    public JsonSerializer(bool ignoreNullValues = true, bool ignoreReadOnlyProperties = true,
        bool propertyNameCaseInsensitive = true, IList<JsonConverter> converters = null)
    {
        options = new JsonSerializerOptions
        {
            IgnoreReadOnlyProperties = ignoreReadOnlyProperties,
            PropertyNameCaseInsensitive = propertyNameCaseInsensitive
        };

        if (ignoreNullValues) options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

        converters?.ToList().ForEach(c => options.Converters.Add(c));
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