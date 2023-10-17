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
    /// 启用审计日志
    /// </summary>
    public bool IsEnabledLog { get; set; }

    /// <summary>
    /// 启用Action审计日志
    /// </summary>
    public bool IsEnabledActionLog { get; set; }

    /// <summary>
    /// 启用异常审计日志
    /// </summary>
    public bool IsEnabledExceptionLog { get; set; }

    /// <summary>
    /// 启用Get请求审计日志
    /// </summary>
    public bool IsEnabledGetRequestLog { get; set; }

    /// <summary>
    /// 允许匿名
    /// </summary>
    public bool AllowAnonymous { get; set; }

    /// <summary>
    /// 日志选择器
    /// </summary>
    public List<Func<AuditLogInfo, Task<bool>>> LogSelectors { get; }

    /// <summary>
    /// 日志贡献者
    /// </summary>
    public List<AuditLogContributor> Contributors { get; }

    /// <summary>
    /// 实体变更配置
    /// </summary>
    public EntityChangeOptions EntityChangeOptions { get; set; }

    public FakeAuditingOptions()
    {
        LogSelectors = new List<Func<AuditLogInfo, Task<bool>>>();
        Contributors = new List<AuditLogContributor>();
        EntityChangeOptions = new EntityChangeOptions();
    }
}

public class EntityChangeOptions
{
    /// <summary>
    /// 启用实体变更审计
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 实体属性变更，新旧值最大长度
    /// </summary>
    public int ValueMaxLength { get; set; }

    /// <summary>
    /// 忽略审计属性
    /// </summary>
    public List<string> IgnoreProperties { get; set; }

    public EntityChangeOptions()
    {
        IgnoreProperties = new List<string>();
    }
}