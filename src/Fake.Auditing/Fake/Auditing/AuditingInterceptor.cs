using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Auditing;

public class AuditingInterceptor: IFakeInterceptor, ITransientDependency
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuditingInterceptor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task InterceptAsync(IFakeMethodInvocation invocation)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        
        await invocation.ProcessAsync();

        var store = scope.ServiceProvider.GetRequiredService<IAuditingStore>();
        await store.SaveAsync(new AuditLogInfo
        {
            ApplicationName = "FAKE"
        });
    }
}