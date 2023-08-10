using Fake.Data;

namespace Fake.AspNetCore.LoadBalancing;

public class FakeServicePikeContext:IHasExtraProperties
{
    /// <summary>
    /// 服务数量
    /// </summary>
    public int ServiceCount { get; set; }
    public ExtraPropertyDictionary ExtraProperties { get; }

    public FakeServicePikeContext()
    {
        ExtraProperties = new ExtraPropertyDictionary();
    }
}