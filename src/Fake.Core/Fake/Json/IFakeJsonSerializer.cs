namespace Fake.Json;

public interface IFakeJsonSerializer
{
    string Serialize(object obj, bool camelCase = true, bool indented = false);

    T Deserialize<T>(string jsonString, bool camelCase = true);
    
    object Deserialize(Type type, string jsonString, bool camelCase = true);
}