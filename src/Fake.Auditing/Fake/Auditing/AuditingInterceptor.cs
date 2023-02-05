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

        if (!auditingHelper.ShouldAuditMethod(invocation.Method))
        {
            await invocation.ProcessAsync();
            return;
        }
        
        var auditingManager = scope.ServiceProvider.GetRequiredService<IAuditingManager>();
        if (auditingManager.Current is null)
        {
            // 使用新的auditing-scope处理
            ProcessWithNewAuditingScopeAsync(invocation, auditingOptions, auditingManager, auditingHelper);
        }
        
        await invocation.ProcessAsync();

        var store = scope.ServiceProvider.GetRequiredService<IAuditingStore>();
        await store.SaveAsync(new AuditLogInfo
        {
            ApplicationName = "FAKE-APP",
            UserName = "FAKE"
        });
    }

    private async Task ProcessWithNewAuditingScopeAsync(IFakeMethodInvocation invocation, FakeAuditingOptions auditingOptions, IAuditingManager auditingManager, IAuditingHelper auditingHelper)
    {
    }
}