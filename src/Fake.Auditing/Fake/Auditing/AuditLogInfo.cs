using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fake.Auditing;

[Serializable]
public class AuditLogInfo
{
    public string ApplicationName { get; set; }
    
    public Guid? UserId { get; set; }

    public string UserName { get; set; }
    
    public DateTime ExecutionTime { get; set; }
    
    public string HttpMethod { get; set; }

    public int? HttpStatusCode { get; set; }

    public string Url { get; set; }
    
    public string ClientIpAddress { get; set; }
    
    public int ExecutionDuration { get; set; }
    
    public List<AuditLogActionInfo> Actions { get; set; }
    
    public List<Exception> Exceptions { get; }
    
    public List<Func<AuditLogInfo, Task<bool>>> LogSelectors { get; }

    public override string ToString()
    {
        return $"""
AUDIT LOG: [{HttpStatusCode?.ToString() ?? "---"}: {HttpMethod ?? "-------",-7}] {Url}
- UserName - UserId      : {UserName} - {UserId}
- ClientIpAddress        : {ClientIpAddress}
- ExecutionDuration      : {ExecutionDuration} ms
""";
    }
}