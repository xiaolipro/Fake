using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.DynamicProxy;
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
        using var serviceScope = _serviceScopeFactory.CreateScope();

        var auditingHelper = serviceScope.ServiceProvider.GetRequiredService<IAuditingHelper>();

        if (!auditingHelper.IsAuditMethod(invocation.Method))
        {
            await invocation.ProcessAsync();
            return;
        }
        
        var auditingManager = serviceScope.ServiceProvider.GetRequiredService<IAuditingManager>();
        var auditingOptions = serviceScope.ServiceProvider.GetRequiredService<IOptions<FakeAuditingOptions>>().Value;
        
        if (auditingManager.Current is null)
        {
            // 使用新的auditing-scope处理
            await ProcessWithNewAuditingScopeAsync(invocation, auditingOptions, auditingManager, auditingHelper);
        }
        
        await invocation.ProcessAsync();

        var store = serviceScope.ServiceProvider.GetRequiredService<IAuditingStore>();
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