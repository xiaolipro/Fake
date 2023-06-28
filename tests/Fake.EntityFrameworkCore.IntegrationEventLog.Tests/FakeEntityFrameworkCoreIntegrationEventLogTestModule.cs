using Fake.Autofac;
using Fake.EntityFrameworkCore.IntegrationEventLog;
using Fake.Modularity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[DependsOn(typeof(FakeEntityFrameworkCoreTestModule))]
[DependsOn(typeof(FakeEntityFrameworkCoreIntegrationEventLogModule))]
public class FakeEntityFrameworkCoreIntegrationEventLogTestModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddDbContextFactory<IntegrationEventLogContext>(builder =>
        {
            //使用sqlite内存模式要开open
            var options = new SqliteConnection("Filename=:memory:");
            options.Open();
            builder.UseSqlite(options).UseLoggerFactory(LoggerFactory.Create(loggingBuilder =>
            {
                loggingBuilder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information) // 仅记录命令信息
                    .AddConsole(); // 输出到控制台
            }));
#if DEBUG
            builder.EnableSensitiveDataLogging();
#endif
            /*builder.UseSqlite("FileName=./fake.db");*/
        });
    }
}