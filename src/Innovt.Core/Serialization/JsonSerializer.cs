// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Innovt.Core.Utilities;

namespace Innovt.Core.Serialization;

/// <summary>
/// Provides JSON serialization and deserialization using System.Text.Json.
/// </summary>
public class JsonSerializer : ISerializer
{
    private readonly JsonSerializerOptions options;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonSerializer"/> class with specified options.
    /// </summary>
    /// <param name="ignoreNullValues">Indicates whether null values should be ignored during serialization.</param>
    /// <param name="ignoreReadOnlyProperties">Indicates whether read-only properties should be ignored during serialization.</param>
    /// <param name="propertyNameCaseInsensitive">Indicates whether property name casing should be treated as case-insensitive.</param>
    /// <param name="converters">A list of custom JSON converters to be used during serialization and deserialization.</param>
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

    /// <summary>
    /// Deserializes a JSON string into an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize.</typeparam>
    /// <param name="serializedObject">The JSON string to deserialize.</param>
    /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
    public T DeserializeObject<T>(string serializedObject)
    {
        return serializedObject.IsNullOrEmpty()
            ? default
            : System.Text.Json.JsonSerializer.Deserialize<T>(serializedObject, options);
    }

    /// <summary>
    /// Serializes an object of type <typeparamref name="T"/> into a JSON string.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>The serialized object as a JSON string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="obj"/> parameter is null.</exception>
    public string SerializeObject<T>(T obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        return System.Text.Json.JsonSerializer.Serialize(obj, options);
    }
}