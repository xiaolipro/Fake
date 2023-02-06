using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.UnitOfWork;

public class UnitOfWorkInterceptor:IFakeInterceptor, ITransientDependency
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UnitOfWorkInterceptor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public Task InterceptAsync(IFakeMethodInvocation invocation)
    {
        throw new System.NotImplementedException();
    }
}