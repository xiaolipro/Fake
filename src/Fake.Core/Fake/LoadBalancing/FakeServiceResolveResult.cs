using System.Collections.Generic;
using Fake.Data;

namespace Fake.AspNetCore.LoadBalancing;

public class FakeServiceResolveResult:IHasExtraProperties
{
    /// <summary>
    /// 服务地址集合
    /// </summary>
    public List<string> ServiceAddressList { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; }

    public FakeServiceResolveResult()
    {
        ServiceAddressList = new List<string>();
        ExtraProperties = new ExtraPropertyDictionary();
    }
}