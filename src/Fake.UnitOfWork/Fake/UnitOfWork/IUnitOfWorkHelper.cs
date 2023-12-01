using System.Reflection;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface IUnitOfWorkHelper
{
    bool IsUnitOfWorkMethod(MethodInfo methodInfo, [CanBeNull] out UnitOfWorkAttribute unitOfWorkAttribute);

    UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MethodInfo methodInfo);
    bool IsReadOnlyUnitOfWorkMethod(MethodInfo invocationMethod);
}