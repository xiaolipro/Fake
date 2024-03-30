using System.Reflection;
using Fake.Modularity;
using Fake.SqlSugarCore;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

[DependsOn(typeof(FakeAppTestModule))]
[DependsOn(typeof(FakeSqlSugarCoreModule))]
public class FakeSqlSugarCoreTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSugarDbContext<OrderingContext>(options =>
        {
            options.ConnectionString = "Filename=:memory:";
            options.DbType = DbType.Sqlite;
            options.EnabledCodeFirst = true;
        });
    }

    public override void ConfigureApplication(ApplicationConfigureContext context)
    {
        var sugarDbContexts = context.ServiceProvider.GetRequiredService<IEnumerable<SugarDbContext>>();
        var moduleContainer = context.ServiceProvider.GetRequiredService<IModuleContainer>();
        foreach (var sugarDbContext in sugarDbContexts)
        {
            if (sugarDbContext.Options.EnabledCodeFirst)
            {
                CodeFirst(sugarDbContext.SqlSugarClient, moduleContainer.Modules);
            }
        }
    }

    private void CodeFirst(ISqlSugarClient client, IReadOnlyList<IModuleDescriptor> modules)
    {
        //尝试创建数据库
        client.DbMaintenance.CreateDatabase();

        var types = new List<Type>();
        foreach (var module in modules)
        {
            types.AddRange(module.Assembly.GetTypes()
                .Where(x => x.GetCustomAttribute<DisableCodeFirstAttribute>() == null)
                .Where(x => x.GetCustomAttribute<SugarTable>() != null)
                .Where(x => x.GetCustomAttribute<SplitTableAttribute>() is null));
        }

        if (types.Count > 0)
        {
            client.CopyNew().CodeFirst.InitTables(types.ToArray());
        }
    }
}