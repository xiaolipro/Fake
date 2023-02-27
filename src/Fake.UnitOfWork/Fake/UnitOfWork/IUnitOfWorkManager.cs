using System;
using Fake.DependencyInjection;
using Fake.Threading;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface IUnitOfWorkManager
{
    [CanBeNull] IUnitOfWork Current { get; }

    IUnitOfWork Begin(UnitOfWorkContext context, bool requiredNew = false);
}

public class AmbientUnitOfWorkProvider : AmbientScopeProvider<IUnitOfWork>
{
    public override IDisposable BeginScope(string contextKey, IUnitOfWork value)
    {
        // 将需要临时存储的对象，用 ScopeItem 包装起来，它的外部对象是当前对象 (如果存在的话)。
        var item = new ScopeItem(value, GetCurrentItemOrNull(contextKey));

        if (!ScopeDictionary.TryAdd(item.Id, item))
        {
            throw new FakeException("无法添加item，ScopeDictionary.TryAdd返回false");
        }

        // 将上下文关联到当前作用域对象
        CallContext.SetData(contextKey, item.Id);

        return new DisposableWrapper(() =>
        {
            ScopeDictionary.TryRemove(item.Id, out item);

            // 将上下文重定向到外部对象，如果没有外部对象，说明离开上下文，直接关联 NULL。
            CallContext.SetData(contextKey, item.Outer?.Id);
        });
    }

    public override IUnitOfWork GetValue(string contextKey)
    {
        var scope = GetCurrentItemOrNull(contextKey);

        IUnitOfWork uow = null;

        while (scope != null)
        {
            uow = scope.Value;
            if (uow.IsCompleted || uow.IsDisposed)
                scope = scope.Outer;
        }

        return uow;
    }
}

public class UnitOfWorkManager : IUnitOfWorkManager, ISingletonDependency
{
    private const string UnitOfWorkContextKey = "Fake.UnitOfWork";

    private readonly AmbientUnitOfWorkProvider _ambientUnitOfWorkProvider;
    public IUnitOfWork Current => _ambientUnitOfWorkProvider.GetValue(UnitOfWorkContextKey);

    public UnitOfWorkManager(AmbientUnitOfWorkProvider ambientUnitOfWorkProvider)
    {
        _ambientUnitOfWorkProvider = ambientUnitOfWorkProvider;
    }

    public IUnitOfWork Begin(UnitOfWorkContext context, bool requiredNew = false)
    {
        if (Current != null && !requiredNew) return Current;
        var scope = _ambientUnitOfWorkProvider.BeginScope(UnitOfWorkContextKey, unitOfWork);
    }
}