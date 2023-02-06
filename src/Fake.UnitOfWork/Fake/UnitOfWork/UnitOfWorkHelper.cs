using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface IUnitOfWorkHelper
{
    bool IsUnitOfWorkMethod([NotNull] MethodInfo methodInfo, [CanBeNull] out UnitOfWorkAttribute unitOfWorkAttribute);

    UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MethodInfo methodInfo);
}

public class UnitOfWorkHelper:IUnitOfWorkHelper
{
    public static bool IsUnitOfWorkType(TypeInfo implementationType)
    {
        //类定义了UnitOfWorkAttribute
        if (implementationType.IsDefined(typeof(UnitOfWorkAttribute)))
        {
            return true;
        }
        
        //类中有实例方法定义了UnitOfWorkAttribute
        if (implementationType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(m =>m.IsDefined(typeof(UnitOfWorkAttribute)))
            )
        {
            return true;
        }

        //实现了IUnitOfWorkEnabled
        if (implementationType.IsAssignableTo(typeof(IUnitOfWorkEnabled)))
        {
            return true;
        }

        return false;
    }

    public bool IsUnitOfWorkMethod(MethodInfo methodInfo, out UnitOfWorkAttribute unitOfWorkAttribute)
    {
        ThrowHelper.ThrowIfNull(methodInfo, nameof(methodInfo));

        unitOfWorkAttribute = GetUnitOfWorkAttributeOrNull(methodInfo);
        return unitOfWorkAttribute is null;
    }

    public UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MethodInfo methodInfo)
    {
        var attr = methodInfo.GetCustomAttribute<UnitOfWorkAttribute>(true);
        return attr ?? methodInfo.DeclaringType.GetTypeInfo().GetCustomAttribute<UnitOfWorkAttribute>(true);
    }
}