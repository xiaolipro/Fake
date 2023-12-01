namespace Fake.LoadBalancing;

public class ServiceResolveResult
{
    public string? Host { get; set; }

    public int Port { get; set; }

    public int GrpcPort { get; set; }
}