using System.Threading;

namespace Fake.UnitOfWork;

public class AmbientUnitOfWorkProvider : IAmbientUnitOfWorkProvider
{
    public IUnitOfWork? UnitOfWork => _currentUow.Value;

    // 核心链路
    private readonly AsyncLocal<IUnitOfWork?> _currentUow = new();

    public void SetUnitOfWork(IUnitOfWork? unitOfWork)
    {
        _currentUow.Value = unitOfWork;
    }

    public IUnitOfWork? GetCurrentByChecking()
    {
        var uow = UnitOfWork;

        // 上溯
        while (uow != null && (uow.IsCompleted || uow.IsDisposed))
        {
            uow = uow.Outer;
        }

        return uow;
    }
}