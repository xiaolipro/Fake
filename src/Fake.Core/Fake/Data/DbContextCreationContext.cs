using System.Data.Common;

namespace Fake.Data;

public class DbContextCreationContext(string connectionString)
{
    public static DbContextCreationContext? Current => LocalContext.Value;

    private static readonly AsyncLocal<DbContextCreationContext?> LocalContext = new();

    public string ConnectionString { get; } = connectionString;

    public DbConnection? ExistingConnection { get; set; }

    public static IDisposable Use(DbContextCreationContext context)
    {
        var previousValue = Current;
        LocalContext.Value = context;
        return new DisposableWrapper(() => LocalContext.Value = previousValue);
    }
}