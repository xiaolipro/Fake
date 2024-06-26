namespace Fake.Data;

public class ConnectionStrings : Dictionary<string, string>
{
    public const string DefaultConnectionStringName = "DefaultConnection";

    public string DefaultConnection
    {
        get => GetConnectionString(DefaultConnectionStringName);
        set => this[DefaultConnectionStringName] = value;
    }

    public string GetConnectionString(string name)
    {
        if (base.TryGetValue(name, out var connectionString))
            return connectionString;

        return string.Empty;
    }
}