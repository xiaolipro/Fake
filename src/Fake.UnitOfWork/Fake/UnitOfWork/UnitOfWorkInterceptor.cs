using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fake.UnitOfWork;

public class UnitOfWorkInterceptor:IFakeInterceptor, ITransientDependency
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UnitOfWorkInterceptor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task InterceptAsync(IFakeMethodInvocation invocation)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        
        var uowHelper = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkHelper>();

        if (!uowHelper.IsUnitOfWorkMethod(invocation.Method, out var unitOfWorkAttribute))
        {
            await invocation.ProcessAsync();
            return;
        }

        var uowOptions = CreateUnitOfWorkOption();
    }

    private FakeUnitOfWorkOptions CreateUnitOfWorkOption()
    {
        var options = new FakeUnitOfWorkOptions();

        return options;
    }
}