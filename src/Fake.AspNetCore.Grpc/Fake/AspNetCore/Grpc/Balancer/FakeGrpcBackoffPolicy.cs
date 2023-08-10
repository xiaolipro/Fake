using System;
using Grpc.Net.Client.Balancer;

namespace Fake.AspNetCore.Grpc.Balancer;

public class FakeGrpcBackoffPolicy : IBackoffPolicy
{
    private readonly FakeGrpcClientOptions _options;

    public FakeGrpcBackoffPolicy(FakeGrpcClientOptions options)
    {
        _options = options;
    }

    public TimeSpan NextBackoff()
    {
        return _options.BackoffInterval;
    }
}