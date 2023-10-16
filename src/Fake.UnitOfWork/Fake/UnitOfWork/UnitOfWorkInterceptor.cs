using System;
using System.Threading.Tasks;
using Fake.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.UnitOfWork;

public class UnitOfWorkInterceptor : IFakeInterceptor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UnitOfWorkInterceptor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public virtual async Task InterceptAsync(IFakeMethodInvocation invocation)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();

        var uowHelper = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkHelper>();

        if (!uowHelper.IsUnitOfWorkMethod(invocation.Method, out var unitOfWorkAttribute))
        {
            await invocation.ProcessAsync();
            return;
        }

        var unitOfWorkManager = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        using var unitOfWork = unitOfWorkManager.Begin(unitOfWorkAttribute);
        await invocation.ProcessAsync();
        if (uowHelper.IsReadOnlyUnitOfWorkMethod(invocation.Method))
        {
            if (unitOfWork.HasHasChanges())
            {
                throw new InvalidOperationException("请不要在只读工作单元内执行查询以外的操作！");
            }
        }
        else
        {
            await unitOfWork.CompleteAsync();
        }
    }
}