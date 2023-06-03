using System;
using System.Linq;
using System.Reflection;

namespace Fake.UnitOfWork;

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
        if (unitOfWorkAttribute is not null) return !unitOfWorkAttribute.IsDisabled;

        return methodInfo.DeclaringType.GetTypeInfo().IsAssignableTo<IUnitOfWorkEnabled>();
    }

    public UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MethodInfo methodInfo)
    {
        // 先从方法上找
        var attr = methodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
        // 再从类上找
        return attr ?? methodInfo.DeclaringType.GetTypeInfo().GetCustomAttribute<UnitOfWorkAttribute>();
    }
}