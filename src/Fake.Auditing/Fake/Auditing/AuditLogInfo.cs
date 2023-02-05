using System;
using System.Text;

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

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"AUDIT LOG: [{HttpStatusCode?.ToString() ?? "---"}: {HttpMethod ?? "-------",-7}] {Url}");
        sb.AppendLine($"- UserName - UserId      : {UserName} - {UserId}");
        sb.AppendLine($"- ClientIpAddress        : {ClientIpAddress}");
        sb.AppendLine($"- ExecutionDuration      : {ExecutionDuration} ms");

        return sb.ToString();
    }
}