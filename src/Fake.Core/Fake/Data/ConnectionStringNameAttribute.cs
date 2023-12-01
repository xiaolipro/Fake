using System.Reflection;

namespace Fake.Data;

public class ConnectionStringNameAttribute : Attribute
{
    public string Name { get; }

    public ConnectionStringNameAttribute(string name)
    {
        ThrowHelper.ThrowIfNull(name, nameof(name));

        Name = name;
    }

    public static string GetConnStringName<T>()
    {
        return GetConnStringName(typeof(T));
    }

    public static string GetConnStringName(Type type)
    {
        var nameAttribute = type.GetTypeInfo().GetCustomAttribute<ConnectionStringNameAttribute>();

        if (nameAttribute == null)
        {
            return type.FullName;
        }

        return nameAttribute.Name;
    }
}