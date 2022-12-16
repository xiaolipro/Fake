using BenchmarkDotNet.Attributes;

namespace Fast.Core.Benchmarks.Helpers.RandomHelperBenchmarks;

public class GUID种子对比默认种子
{
    private static readonly ThreadLocal<Random> LocalRandom = new(() => new Random(Guid.NewGuid().GetHashCode()));
    private static readonly ThreadLocal<Random> LocalRandom2 = new(() => new Random());
    
    [Benchmark]
    public void GUID()
    {
        LocalRandom.Value.Next();
    }
    
    [Benchmark]
    public void 默认()
    {
        LocalRandom2.Value.Next();
    }
    
    [Benchmark]
    public void GUID并行()
    {
        Enumerable.Range(0, 100_0000).AsParallel()
            .Select(i => LocalRandom.Value.Next())
            .ToArray();
    }
    
    [Benchmark]
    public void 默认并行()
    {
        Enumerable.Range(0, 100_0000).AsParallel()
            .Select(i => LocalRandom2.Value.Next())
            .ToArray();
    }
}