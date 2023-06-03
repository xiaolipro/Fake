using System;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.UnitOfWork;

public class UnitOfWorkManager : IUnitOfWorkManager
{
    private readonly IAmbientUnitOfWorkProvider _ambientUnitOfWorkProvider;
    private readonly IServiceProvider _serviceProvider;
    public IUnitOfWork Current => _ambientUnitOfWorkProvider.GetCurrentByChecking();

    public UnitOfWorkManager(IAmbientUnitOfWorkProvider ambientUnitOfWorkProvider,
        IServiceProvider serviceProvider)
    {
        _ambientUnitOfWorkProvider = ambientUnitOfWorkProvider;
        _serviceProvider = serviceProvider;
    }

    public IUnitOfWork Begin(UnitOfWorkAttribute attribute)
    {
        var currentUow = Current;
        var requiresNew = attribute?.RequiresNew ?? false;
        if (currentUow != null && !requiresNew)
        {
            return new ChildUnitOfWork(currentUow);
        }

        var unitOfWork = CreateNewUnitOfWork();
        unitOfWork.InitUnitOfWorkContext(attribute);

        return unitOfWork;
    }

    private IUnitOfWork CreateNewUnitOfWork()
    {
        var outerUow = Current;

        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();

        unitOfWork.SetOuter(outerUow);

        _ambientUnitOfWorkProvider.SetUnitOfWork(unitOfWork);

        unitOfWork.Disposed += (_, _) =>
        {
            // 重定向到外层工作单元
            _ambientUnitOfWorkProvider.SetUnitOfWork(outerUow);
        };

        return unitOfWork;
    }
}