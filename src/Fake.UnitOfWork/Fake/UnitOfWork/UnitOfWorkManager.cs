using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.UnitOfWork;

public class UnitOfWorkManager : IUnitOfWorkManager
{
    private const string UnitOfWorkContextKey = "Fake.UnitOfWork";

    private readonly AmbientUnitOfWorkProvider _ambientUnitOfWorkProvider;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public IUnitOfWork Current => _ambientUnitOfWorkProvider.GetValue(UnitOfWorkContextKey);

    public UnitOfWorkManager(AmbientUnitOfWorkProvider ambientUnitOfWorkProvider,
        IServiceScopeFactory serviceScopeFactory)
    {
        _ambientUnitOfWorkProvider = ambientUnitOfWorkProvider;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public IUnitOfWork Begin(UnitOfWorkAttribute attribute, bool requiredNew = false)
    {
        if (Current != null && !requiredNew) return Current;

        var unitOfWork = CreateNewUnitOfWork();
        unitOfWork.InitUnitOfWorkContext(attribute);
        
        var scope = _ambientUnitOfWorkProvider.BeginScope(UnitOfWorkContextKey, unitOfWork);
        
        unitOfWork.OnDisposed(_ =>
        {
            scope.Dispose();
            return Task.CompletedTask;
        });

        return unitOfWork;
    }

    private IUnitOfWork CreateNewUnitOfWork()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        
        var outerUow = Current;

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        unitOfWork.SetOuter(outerUow);

        return unitOfWork;
    }
}