using FreeRedis;

namespace Fake.Caching;

public interface IRedisInitializer
{
    RedisClient Initialize();
}