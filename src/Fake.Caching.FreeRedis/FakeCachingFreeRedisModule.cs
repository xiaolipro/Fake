using Fake.Caching;
using Fake.Modularity;
using FreeRedis;
using Microsoft.Extensions.DependencyInjection;

public class FakeCachingFreeRedisModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IRedisInitializer, RedisInitializer>();

        context.Services.AddSingleton<IRedisClient, RedisClient>(serviceProvider =>
            serviceProvider.GetRequiredService<IRedisInitializer>().Initialize());
    }
}