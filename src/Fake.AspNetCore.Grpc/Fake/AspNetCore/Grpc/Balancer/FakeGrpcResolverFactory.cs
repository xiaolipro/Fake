using Fake.LoadBalancing;
using Grpc.Net.Client.Balancer;
using Microsoft.Extensions.Options;

namespace Fake.AspNetCore.Grpc.Balancer;

public class FakeGrpcResolverFactory : ResolverFactory
{
    private readonly IServiceBalancer _serviceBalancer;
    private readonly IBackoffPolicyFactory _backoffPolicyFactory;
    private readonly FakeGrpcClientOptions _options;

    public override string Name => nameof(FakeGrpcResolverFactory);

    public FakeGrpcResolverFactory(IBackoffPolicyFactory backoffPolicyFactory, IServiceBalancer serviceBalancer, IOptions<FakeGrpcClientOptions> options)
    {
        _backoffPolicyFactory = backoffPolicyFactory;
        _serviceBalancer = serviceBalancer;
        _options = options.Value;
    }

    public override Resolver Create(ResolverOptions options)
    {
        return new FakeGrpcResolver(options.LoggerFactory, _backoffPolicyFactory, options.Address, _serviceBalancer, _options);
    }
}