using Fake.Auditing;
using Fake.DependencyInjection;
using Fake.DomainDrivenDesign.Application;
using Microsoft.AspNetCore.Mvc;

namespace SimpleWebDemo;

/// <summary>
/// 测试德玛
/// </summary>
[Audited]
[ApiExplorerSettings(GroupName = "德玛西亚")]
public class TestApplicationService : ApplicationService, ITransientDependency
{
    /// <summary>
    /// 德玛API
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual string Hello(string name)
    {
        return $"hello {name}";
    }

    /// <summary>
    /// 德玛API2
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual async Task<string> Hello2(string name)
    {
        await Task.Delay(1010);
        return $"hello {name}";
    }
}