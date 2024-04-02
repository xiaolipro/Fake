using Fake.Modularity;
using Fake.SqlSugarCore;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

[DependsOn(typeof(FakeSqlSugarCoreModule))]
public class FakeSqlSugarCoreTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSugarDbContext<OrderingContext>(options =>
        {
            options.ConnectionString = "Filename=:memory:";
            options.DbType = DbType.Sqlite;
        });
    }
}