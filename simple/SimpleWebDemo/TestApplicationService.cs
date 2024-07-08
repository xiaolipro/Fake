using Fake.Application;
using Fake.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace SimpleWebDemo;

/// <summary>
/// 测试德玛
/// </summary>
[ApiExplorerSettings(GroupName = "德玛西亚")]
public class TestApplicationService : ApplicationService
{
    /// <summary>
    /// 德玛API
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [Audited]
    public virtual string Hello(string name)
    {
        Logger.LogInformation("突突你");
        return $"hello {name}";
    }

    public string GetName(string lastName, string firstName)
    {
        Logger.LogError("突突你d");
        return $"{firstName} {lastName}";
    }

    /// <summary>
    /// 德玛API2
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    public virtual async Task<string> Hello2(string name)
    {
        await Task.Delay(1010);
        return $"hello {name}";
    }
}