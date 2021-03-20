namespace Innovt.Core.Serialization
{
    public interface ISerializer
    {
        T DeserializeObject<T>(string serializedObject);

        string SerializeObject<T>(T obj);
    }
}