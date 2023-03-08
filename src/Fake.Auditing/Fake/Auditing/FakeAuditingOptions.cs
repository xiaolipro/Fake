using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fake.Auditing;

public class FakeAuditingOptions
{
    /// <summary>
    /// 应用名称
    /// </summary>
    public string ApplicationName { get; set; }
    
    /// <summary>
    /// 启用基础审计日志，默认: true
    /// </summary>
    public bool IsEnabledLog { get; set; }

    /// <summary>
    /// 启用Action审计日志，默认: true
    /// </summary>
    public bool IsEnabledActionLog { get; set; }
    
    /// <summary>
    /// 启用异常审计日志，默认: true
    /// </summary>
    public bool IsEnabledExceptionLog{ get; set; }
    
    /// <summary>
    /// 启用Get请求审计日志，默认: true
    /// </summary>
    public bool IsEnabledGetRequestLog{ get; set; }
    
    /// <summary>
    /// 日志选择器
    /// </summary>
    public List<Func<AuditLogInfo, Task<bool>>> LogSelectors { get; }
    
    /// <summary>
    /// 日志贡献者
    /// </summary>
    public List<AuditLogContributor> Contributors { get; }

    public FakeAuditingOptions()
    {
        LogSelectors = new List<Func<AuditLogInfo, Task<bool>>>();
        Contributors = new List<AuditLogContributor>();
    }
}