using BenchmarkDotNet.Attributes;

namespace Fast.Core.Benchmarks.Helpers.RandomHelperBenchmarks;

public class 自定义种子对比默认种子
{
    private static readonly ThreadLocal<Random> LocalRandom = new(() => new Random(Guid.NewGuid().GetHashCode()));
    private static readonly ThreadLocal<Random> LocalRandom2 = new(() => new Random());
    
    [Benchmark]
    public void 自定义()
    {
        LocalRandom.Value.Next();
    }
    
    [Benchmark]
    public void 默认()
    {
        LocalRandom2.Value.Next();
    }
}