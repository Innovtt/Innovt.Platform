// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

namespace Innovt.Core.Serialization;

public interface ISerializer
{
    T DeserializeObject<T>(string serializedObject);

    string SerializeObject<T>(T obj);
}