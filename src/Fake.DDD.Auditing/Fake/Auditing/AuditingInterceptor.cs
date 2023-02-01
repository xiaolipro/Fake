using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.Proxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

        var auditingHelper = scope.ServiceProvider.GetRequiredService<IAuditingHelper>();
        var auditingOptions = scope.ServiceProvider.GetRequiredService<IOptions<FakeAuditingOptions>>().Value;

        if (!auditingHelper.ShouldSaveAudit(invocation.Method))
        {
            await invocation.ProcessAsync();
            return;
        }
        
        await invocation.ProcessAsync();

        var store = scope.ServiceProvider.GetRequiredService<IAuditingStore>();
        await store.SaveAsync(new AuditLogInfo
        {
            ApplicationName = "FAKE"
        });
    }
}