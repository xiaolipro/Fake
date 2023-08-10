using Fake.AspNetCore.LoadBalancing;
using Grpc.Net.Client.Balancer;

namespace Fake.AspNetCore.Grpc.Balancer;

public class FakeGrpcBalancerFactory : LoadBalancerFactory
{
    private readonly ILoadBalancer _balancer;

    public override string Name => nameof(FakeGrpcBalancerFactory);

    public FakeGrpcBalancerFactory(ILoadBalancer balancer)
    {
        _balancer = balancer;
    }

    public override LoadBalancer Create(LoadBalancerOptions options)
    {
        return new FakeGrpcBalancer(options.Controller, options.LoggerFactory, _balancer);
    }
}