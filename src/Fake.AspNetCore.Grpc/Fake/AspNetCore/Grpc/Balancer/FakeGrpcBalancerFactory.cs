using Fake.LoadBalancing;
using Grpc.Net.Client.Balancer;

namespace Fake.AspNetCore.Grpc.Balancer;

public class FakeGrpcBalancerFactory : LoadBalancerFactory
{
    private readonly IFakeBalancer _balancer;

    public override string Name => nameof(FakeGrpcBalancerFactory);

    public FakeGrpcBalancerFactory(IFakeBalancer balancer)
    {
        _balancer = balancer;
    }

    public override LoadBalancer Create(LoadBalancerOptions options)
    {
        return new FakeGrpcBalancer(options.Controller, options.LoggerFactory, _balancer);
    }
}