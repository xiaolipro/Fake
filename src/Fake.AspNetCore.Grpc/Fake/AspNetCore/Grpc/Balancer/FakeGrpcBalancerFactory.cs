using Fake.LoadBalancing;
using Grpc.Net.Client.Balancer;

namespace Fake.AspNetCore.Grpc.Balancer;

public class FakeGrpcBalancerFactory : LoadBalancerFactory
{
    private readonly IServiceBalancer _serviceBalancer;

    public override string Name => nameof(FakeGrpcBalancerFactory);

    public FakeGrpcBalancerFactory(IServiceBalancer serviceBalancer)
    {
        _serviceBalancer = serviceBalancer;
    }

    public override LoadBalancer Create(LoadBalancerOptions options)
    {
        return new FakeGrpcBalancer(options.Controller, options.LoggerFactory, _serviceBalancer);
    }
}