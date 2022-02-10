// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Innovt.Core.Serialization
{
    public class XmlSerializer : ISerializer
    {
        public T DeserializeObject<T>(string serializedObject)
        {
            using var xmlReader = XmlReader.Create(new StringReader(serializedObject));
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(xmlReader);
        }

        public string SerializeObject<T>(T obj)
        {
            return SerializeObject(obj, null);
        }

        public string SerializeObject<T>(T obj, Dictionary<string, string> namespaces)
        {
            try
            {
                XmlSerializerNamespaces xmlSerializerNamespaces = null;

                if (namespaces != null)
                {
                    xmlSerializerNamespaces = new XmlSerializerNamespaces();
                    xmlSerializerNamespaces.Add(string.Empty, string.Empty);
                    foreach (var np in namespaces) xmlSerializerNamespaces.Add(np.Key, np.Value);
                }

                using var memoryStream = new MemoryStream();
                using var writer = new XmlTextWriter(memoryStream, Encoding.UTF8);

                var serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType(), "");
                if (xmlSerializerNamespaces != null)
                {
                    writer.Namespaces = true;
                    serializer.Serialize(writer, obj, xmlSerializerNamespaces);
                }
                else
                {
                    serializer.Serialize(writer, obj);
                }

                writer.Close();
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch (XmlException)
            {
                throw new Exception("Xml Serialization error.");
            }
        }
    }
}