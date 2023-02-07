namespace Fake.Auditing;

public class FakeAuditingOptions
{
    /// <summary>
    /// The name of the application or service writing audit logs.
    /// Default: null.
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
}