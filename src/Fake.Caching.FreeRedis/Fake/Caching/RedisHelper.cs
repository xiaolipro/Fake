using FreeRedis;

namespace Fake.Caching;

public static class RedisHelper
{
    public static IRedisClient Client { get; private set; } = null!;

    internal static void SetClient(IRedisClient client)
    {
        Client = client;
    }
}