using Fake.Threading;

namespace Fake.UnitOfWork;

public class AmbientUnitOfWorkProvider : AmbientScopeProvider<IUnitOfWork>
{
    public override IUnitOfWork GetValue(string contextKey)
    {
        var scope = GetCurrentItemOrNull(contextKey);

        IUnitOfWork uow = null;

        while (scope != null)
        {
            uow = scope.Value;
            if (uow.UnitOfWorkStatus is UnitOfWorkStatus.Completed)
                scope = scope.Outer;
        }

        return uow;
    }
}