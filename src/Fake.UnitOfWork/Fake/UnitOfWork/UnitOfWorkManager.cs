using Microsoft.Extensions.DependencyInjection;

namespace Fake.UnitOfWork;

public class UnitOfWorkManager : IUnitOfWorkManager
{
    private readonly IAmbientUnitOfWorkProvider _ambientUnitOfWorkProvider;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public IUnitOfWork? Current => _ambientUnitOfWorkProvider.GetCurrentByChecking();

    public UnitOfWorkManager(IAmbientUnitOfWorkProvider ambientUnitOfWorkProvider,
        IServiceScopeFactory serviceScopeFactory)
    {
        _ambientUnitOfWorkProvider = ambientUnitOfWorkProvider;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public IUnitOfWork Begin(UnitOfWorkAttribute? attribute)
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
        var scope = _serviceScopeFactory.CreateScope();
        try
        {
            var outerUow = Current;

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            unitOfWork.SetOuter(outerUow);

            _ambientUnitOfWorkProvider.SetUnitOfWork(unitOfWork);

            var localScope = scope; // 将scope拷贝到局部变量
            unitOfWork.Disposed += (_, _) =>
            {
                // 重定向到外层工作单元
                _ambientUnitOfWorkProvider.SetUnitOfWork(outerUow);
                localScope.Dispose();
            };

            return unitOfWork;
        }
        catch
        {
            scope.Dispose();
            throw;
        }
    }
}