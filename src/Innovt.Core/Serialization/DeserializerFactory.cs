// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Innovt.Core.Utilities;

namespace Innovt.Core.Serialization;

/// <summary>
/// Represents a factory for deserializing JSON content into objects based on their registered type mappings.
/// </summary>
public class DeserializerFactory {
    private static DeserializerFactory instance;

    private readonly Dictionary<string, Type> mapping;

    private DeserializerFactory() {
        mapping = new Dictionary<string, Type>();
    }

    /// <summary>
    /// Gets the singleton instance of the <see cref="DeserializerFactory"/>.
    /// </summary>
    public static DeserializerFactory Instance => instance ??= new DeserializerFactory();

    private string GetGenericFullName<T>() {
        return typeof(T).FullName;
    }

    /// <summary>
    /// Adds a type mapping to the factory, associating a key with a specific .NET type.
    /// </summary>
    /// <typeparam name="T">The .NET type to be associated with the key.</typeparam>
    /// <param name="key">The key used to identify the type mapping. If not provided, the full name of the type will be used as the key.</param>
    /// <returns>The current <see cref="DeserializerFactory"/> instance for method chaining.</returns>
    public DeserializerFactory AddMapping<T>(string key = null) {
        var typeKey = key ?? GetGenericFullName<T>();

        mapping.TryAdd(typeKey, typeof(T));
        return instance;
    }

    /// <summary>
    /// Deserializes JSON content into an object of the registered type associated with the specified key.
    /// </summary>
    /// <param name="key">The key associated with the registered type mapping.</param>
    /// <param name="content">The JSON content to deserialize.</param>
    /// <returns>The deserialized object of the registered type, or <c>null</c> if the key is not found or the content is empty.</returns>
    public object Deserialize([NotNull] string key, string content) {
        if (key == null) throw new ArgumentNullException(nameof(key));

        if (string.IsNullOrEmpty(content))
            return null;

        if (!mapping.TryGetValue(key, out var typeValue))
            return null;

        var deserialized = System.Text.Json.JsonSerializer.Deserialize(content, typeValue);

        return deserialized;
    }

    /// <summary>
    /// Deserializes JSON content into an object of the registered type associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The expected type of the deserialized object.</typeparam>
    /// <param name="key">The key associated with the registered type mapping.</param>
    /// <param name="content">The JSON content to deserialize.</param>
    /// <returns>The deserialized object of the specified type, or the default value for <typeparamref name="T"/> if the key is not found or the content is empty.</returns>
    public T Deserialize<T>([NotNull] string key, string content) {
        return (T)Deserialize(key, content);
    }
}