using System;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface IUnitOfWorkManager
{
    [CanBeNull] IUnitOfWork Current { get; }

    IUnitOfWork Begin(UnitOfWorkContext context, bool requiredNew = false);
}