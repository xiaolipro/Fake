namespace Fake.Auditing;

public class FakeAuditingOptions
{
    /// <summary>
    /// The name of the application or service writing audit logs.
    /// Default: null.
    /// </summary>
    public string ApplicationName { get; set; }
    
    /// <summary>
    /// 启用审计，默认: true
    /// </summary>
    public bool IsEnabled { get; set; }
}