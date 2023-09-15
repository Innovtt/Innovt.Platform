// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

namespace Innovt.Core.Serialization;

/// <summary>
/// Represents an interface for serializing and deserializing objects.
/// </summary>
public interface ISerializer {
    /// <summary>
    /// Deserializes a serialized object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize.</typeparam>
    /// <param name="serializedObject">The serialized object as a string.</param>
    /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
    T DeserializeObject<T>(string serializedObject);

    /// <summary>
    /// Serializes an object of type <typeparamref name="T"/> to a string.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>The serialized object as a string.</returns>
    string SerializeObject<T>(T obj);
}
