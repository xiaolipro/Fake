using Fake.Json;
using FreeRedis;
using Microsoft.Extensions.Configuration;

namespace Fake.Caching;

public class RedisInitializer(IConfiguration configuration, IFakeJsonSerializer jsonSerializer) : IRedisInitializer
{
    public RedisClient Initialize()
    {
        var options = configuration.GetSection("Redis").Get<ConnectionStringBuilder[]>();
        if (options == null)
        {
            throw new FakeException("未找到Redis配置");
        }

        var client = new RedisClient(options);
        client.Serialize = obj => jsonSerializer.Serialize(obj);
        client.Deserialize = (data, type) => jsonSerializer.Deserialize(data, type);

        return client;
    }
}