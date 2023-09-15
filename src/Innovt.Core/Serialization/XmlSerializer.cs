// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using Innovt.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Innovt.Core.Serialization;

/// <summary>
/// Provides XML serialization and deserialization using System.Xml.Serialization.
/// </summary>
public class XmlSerializer : ISerializer {
    /// <summary>
    /// Deserializes an XML string into an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize.</typeparam>
    /// <param name="serializedObject">The XML string to deserialize.</param>
    /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
    public T DeserializeObject<T>(string serializedObject) {
        using var xmlReader = XmlReader.Create(new StringReader(serializedObject));
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

        return (T)serializer.Deserialize(xmlReader);
    }

    /// <summary>
    /// Serializes an object of type <typeparamref name="T"/> into an XML string.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>The serialized object as an XML string.</returns>
    public string SerializeObject<T>(T obj) {
        return SerializeObject(obj, null);
    }

    /// <summary>
    /// Serializes an object of type <typeparamref name="T"/> into an XML string with optional namespaces.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="namespaces">A dictionary of namespaces to include in the XML serialization.</param>
    /// <returns>The serialized object as an XML string.</returns>
    /// <exception cref="Exception">Thrown when there is an XML serialization error.</exception>
    public string SerializeObject<T>(T obj, Dictionary<string, string> namespaces) {
        try {
            XmlSerializerNamespaces xmlSerializerNamespaces = null;

            if (namespaces != null) {
                xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);
                foreach (var np in namespaces) xmlSerializerNamespaces.Add(np.Key, np.Value);
            }

            using var memoryStream = new MemoryStream();
            using var writer = new XmlTextWriter(memoryStream, Encoding.UTF8);

            var serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType(), "");
            if (xmlSerializerNamespaces != null) {
                writer.Namespaces = true;
                serializer.Serialize(writer, obj, xmlSerializerNamespaces);
            }
            else {
                serializer.Serialize(writer, obj);
            }

            writer.Close();
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
        catch (XmlException) {
            throw new CriticalException("XML Serialization error.");
        }

        
    }
}