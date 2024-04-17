using System.Reflection;

namespace Fake.UnitOfWork;

public interface IUnitOfWorkHelper
{
    bool IsUnitOfWorkMethod(MethodInfo methodInfo, out UnitOfWorkAttribute? unitOfWorkAttribute);

    UnitOfWorkAttribute? GetUnitOfWorkAttributeOrNull(MethodInfo methodInfo);
    bool IsReadOnlyUnitOfWorkMethod(MethodInfo invocationMethod);
}