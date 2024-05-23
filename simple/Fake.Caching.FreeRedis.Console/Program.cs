// See https://aka.ms/new-console-template for more information

using FreeRedis;

var cli = new RedisClient(
    "redis-dev.default.svc.cluster.local:6379,password=gaVEHR3V7qiY,allowAdmin=true,MaxPoolSize=1");

await Parallel.ForEachAsync(Enumerable.Range(1, 400000), new ParallelOptions()
{
    MaxDegreeOfParallelism = 100
}, async (i, token) =>
{
    await cli.SetAsync("abc", 1);
    var r = await cli.ExistsAsync("abc");
    cli.SetAsync("abc", 1);
    cli.SetAsync("abc", 1);
    cli.SetAsync("abc", 1);
    cli.SetAsync("abc", 1);
    cli.SetAsync("abc", 1);
    Console.WriteLine(r + "" + i);
});