using Fake.Threading;

namespace Fake.UnitOfWork;

public class AmbientUnitOfWorkProvider : AmbientScopeProvider<IUnitOfWork>
{
    public override IUnitOfWork GetValue(string contextKey)
    {
        var scope = GetCurrentItemOrNull(contextKey);

        IUnitOfWork uow = uow = scope?.Value;

        // 上溯
        while (uow is { UnitOfWorkStatus: UnitOfWorkStatus.Completed })
        {
            scope = scope.Outer;
            uow = scope?.Value;
        }

        // 将上下文关联到当前作用域对象
        CallContext.SetData(contextKey, scope?.Id);
        return uow;
    }
}