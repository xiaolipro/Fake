using Fake.LoadBalancing;
using Grpc.Net.Client.Balancer;
using Microsoft.Extensions.Options;

namespace Fake.AspNetCore.Grpc.Balancer;

public class FakeGrpcResolverFactory : ResolverFactory
{
    private readonly IFakeBalancer _balancer;
    private readonly IBackoffPolicyFactory _backoffPolicyFactory;
    private readonly FakeGrpcClientOptions _options;

    public override string Name => nameof(FakeGrpcResolverFactory);

    public FakeGrpcResolverFactory(IBackoffPolicyFactory backoffPolicyFactory, IFakeBalancer balancer, IOptions<FakeGrpcClientOptions> options)
    {
        _backoffPolicyFactory = backoffPolicyFactory;
        _balancer = balancer;
        _options = options.Value;
    }

    public override Resolver Create(ResolverOptions options)
    {
        return new FakeGrpcResolver(options.LoggerFactory, _backoffPolicyFactory, options.Address, _balancer, _options);
    }
}