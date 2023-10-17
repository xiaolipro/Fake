using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fake.Auditing;

[Serializable]
public class AuditLogInfo
{
    public string ApplicationName { get; set; }

    public string UserId { get; set; }

    public string UserName { get; set; }

    public DateTime ExecutionTime { get; set; }

    public string HttpMethod { get; set; }

    public int? HttpStatusCode { get; set; }

    public string Url { get; set; }

    public string ClientIpAddress { get; set; }

    public string UserAgent { get; set; }

    public int ExecutionDuration { get; set; }

    public List<AuditLogActionInfo> Actions { get; set; }

    public List<Exception> Exceptions { get; }

    public List<EntityChangeInfo> EntityChanges { get; set; }

    public AuditLogInfo()
    {
        Actions = new List<AuditLogActionInfo>();
        Exceptions = new List<Exception>();
        EntityChanges = new List<EntityChangeInfo>();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"AUDIT LOG: [{HttpStatusCode?.ToString() ?? "---"}: {HttpMethod ?? "-------",-7}] {Url}");
        sb.AppendLine($"- UserName - UserId      : {UserName} - {UserId}");
        sb.AppendLine($"- ClientIpAddress        : {ClientIpAddress}");
        sb.AppendLine($"- ExecutionDuration      : {ExecutionDuration} ms");

        if (Actions.Any())
        {
            sb.AppendLine("- Actions:");
            foreach (var action in Actions)
            {
                sb.AppendLine($"  - {action.ServiceName}.{action.MethodName} ({action.ExecutionDuration} ms.)");
                sb.AppendLine($"    {action.Parameters}");
            }
        }

        if (Exceptions.Any())
        {
            sb.AppendLine("- Exceptions:");
            foreach (var exception in Exceptions)
            {
                sb.AppendLine($"  - {exception.Message}");
                sb.AppendLine($"    {exception}");
            }
        }

        if (EntityChanges.Any())
        {
            sb.AppendLine("- Entity Changes:");
            foreach (var entityChange in EntityChanges)
            {
                sb.AppendLine(
                    $"  - [{entityChange.ChangeType}] {entityChange.EntityTypeFullName}, Id = {entityChange.EntityId}");
                foreach (var propertyChange in entityChange.PropertyChanges)
                {
                    sb.AppendLine(
                        $"    {propertyChange.PropertyName}: {propertyChange.OriginalValue} -> {propertyChange.NewValue}");
                }
            }
        }

        return sb.ToString();
    }
}