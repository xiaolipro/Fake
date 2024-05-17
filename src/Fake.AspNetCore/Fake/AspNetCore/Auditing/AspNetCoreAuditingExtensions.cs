using Fake.Auditing;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fake.AspNetCore.Auditing;

public static class AspNetCoreAuditingExtensions
{
    public static IServiceCollection AddFakeAspNetCoreAuditing(this IServiceCollection services)
    {
        services.Configure<FakeAuditingOptions>(options =>
        {
            options.Contributors.Add(new AspNetCoreAuditLogContributor());
        });

        services.Replace(ServiceDescriptor.Singleton<IAuditingStore, AspNetCoreAuditingStore>());

        return services;
    }
}