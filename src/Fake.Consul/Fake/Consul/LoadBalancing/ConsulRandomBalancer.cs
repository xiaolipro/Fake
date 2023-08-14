using Consul;
using Fake.Helpers;
using Microsoft.Extensions.Logging;

namespace Fake.Consul.LoadBalancing;

public class ConsulRandomBalancer : AbstractConsulServiceBalancer
{
    public ConsulRandomBalancer(ILogger<ConsulRandomBalancer> logger, IConsulClient client) : base(logger, client)
    {
    }

    protected override int PickIndex(string serviceName)
    {
        return RandomHelper.Next(0, ServiceNodes[serviceName].Count);
    }
}