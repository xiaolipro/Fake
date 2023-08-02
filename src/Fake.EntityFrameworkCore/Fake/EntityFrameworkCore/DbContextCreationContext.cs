using System;
using System.Data.Common;
using System.Threading;
using Fake.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Fake.EntityFrameworkCore;

public class DbContextCreationContext
{
    public static DbContextCreationContext Current => _current.Value;
    
    private static readonly AsyncLocal<DbContextCreationContext> _current = new AsyncLocal<DbContextCreationContext>();

    public string ConnectionString { get; }

    public DbConnection ExistingConnection { get; internal set; }

    public DbContextCreationContext(string connectionString)
    {
        ConnectionString = connectionString;
    }
    
    public static IDisposable Use(DbContextCreationContext context)
    {
        var previousValue = Current;
        _current.Value = context;
        return new DisposableWrapper(() => _current.Value = previousValue);
    }


    public static DbContextCreationContext GetCreationContext<TDbContext>(IConfiguration configuration)
        where TDbContext : DbContext
    {
        if (Current != null) return Current;

        var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();
        var connectionString = configuration.GetConnectionString(connectionStringName);
        return new DbContextCreationContext(connectionString);
    }
}