
using Fake.AspNetCore.Grpc;
using Fake.AspNetCore.Grpc.Interceptors;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;
using SkyApm.Diagnostics.Grpc.Server;

public class FakeAspNetCoreGrpcModule:FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}