using System;
using System.Threading.Tasks;
using Fake.DependencyInjection;
using Fake.DynamicProxy;
using JetBrains.Annotations;
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

        var context = CreateUnitOfWorkContext(invocation, serviceScope.ServiceProvider, unitOfWorkAttribute);
        var unitOfWorkManager = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    }

    private UnitOfWorkContext CreateUnitOfWorkContext(IFakeMethodInvocation invocation,IServiceProvider serviceProvider, [CanBeNull] UnitOfWorkAttribute unitOfWorkAttribute)
    {
        var context = new UnitOfWorkContext();

        if (unitOfWorkAttribute is not null)
        {
            context.IsolationLevel = unitOfWorkAttribute.IsolationLevel;
            context.Timeout = unitOfWorkAttribute.Timeout;

            var unitOfWorkOptions = serviceProvider.GetRequiredService<IOptions<FakeUnitOfWorkOptions>>().Value;
            var isTransactional = serviceProvider.GetRequiredService<NullUnitOfWorkIsTransactionalProvider>()
                .IsTransactional ?? !invocation.Method.Name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase);
            context.IsTransactional = unitOfWorkOptions.IsTransactional(isTransactional);
        }
        
        return context;
    }
}

public interface IUnitOfWorkIsTransactionalProvider
{
    bool? IsTransactional { get; }
}