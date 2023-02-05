using System.Reflection;

namespace Fake.UnitOfWork;

public static class UnitOfWorkHelper
{
    public static bool IsUnitOfWorkType(TypeInfo implementationType)
    {
        //Explicitly defined UnitOfWorkAttribute
        if (HasUnitOfWorkAttribute(implementationType) || AnyMethodHasUnitOfWorkAttribute(implementationType))
        {
            return true;
        }

        //Conventional classes
        if (typeof(IUnitOfWorkEnabled).GetTypeInfo().IsAssignableFrom(implementationType))
        {
            return true;
        }

        return false;
    }
    
    private static bool HasUnitOfWorkAttribute(MemberInfo methodInfo)
    {
        return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
    }
}