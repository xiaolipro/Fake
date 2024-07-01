namespace Fake.Data;

public class ConnectionStrings : Dictionary<string, string>
{
    public const string DefaultConnectionStringName = "Default";

    public string DefaultConnectionString
    {
        get => GetConnectionString(DefaultConnectionStringName);
        set => this[DefaultConnectionStringName] = value;
    }

    public string GetConnectionString(string name)
    {
        return TryGetValue(name, out var connectionString) ? connectionString : string.Empty;
    }
}