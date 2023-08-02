using BenchmarkDotNet.Attributes;

namespace Fake.Core.Benchmarks.Helpers.RandomHelperBenchmarks;

public class GUID种子对比默认种子
{
    private static readonly ThreadLocal<Random> LocalRandom = new(() => new Random(Guid.NewGuid().GetHashCode()));
    private static readonly ThreadLocal<Random> LocalRandom2 = new(() => new Random());
    
    [Benchmark]
    public void GUID()
    {
        LocalRandom.Value?.Next();
    }
    
    [Benchmark]
    public void 默认()
    {
        LocalRandom2.Value?.Next();
    }
    
    [Benchmark]
    public void Guid并行()
    {
        Enumerable.Range(0, 100_0000).AsParallel()
            .Select(_ => LocalRandom.Value!.Next())
            .ToArray();
    }
    
    [Benchmark]
    public void 默认并行()
    {
        Enumerable.Range(0, 100_0000).AsParallel()
            .Select(_ => LocalRandom2.Value!.Next())
            .ToArray();
    }
}