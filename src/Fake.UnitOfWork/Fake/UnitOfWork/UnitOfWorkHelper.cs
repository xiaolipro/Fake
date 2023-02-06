using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface IUnitOfWorkHelper
{
    bool IsUnitOfWorkType(TypeInfo implementationType);

    bool IsUnitOfWorkMethod([NotNull] MethodInfo methodInfo, [CanBeNull] out UnitOfWorkAttribute unitOfWorkAttribute);

    UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MethodInfo methodInfo);
}

public class UnitOfWorkHelper:IUnitOfWorkHelper
{
    public bool IsUnitOfWorkType(TypeInfo implementationType)
    {
        //类定义了UnitOfWorkAttribute
        if (implementationType.IsDefined(typeof(UnitOfWorkAttribute)))
        {
            return true;
        }
        
        //类中有方法定义了UnitOfWorkAttribute
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
        
        //方法定义了UnitOfWorkAttribute
        if (methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true))
        {
            return IsUnitOfWorkMember(methodInfo, out unitOfWorkAttribute);
        }

        var declaringType = methodInfo.DeclaringType;
        return IsUnitOfWorkMember(declaringType, out unitOfWorkAttribute);
    }

    public UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MethodInfo methodInfo)
    {
        throw new NotImplementedException();
    }

    private bool IsUnitOfWorkMember(MemberInfo methodInfo, out UnitOfWorkAttribute unitOfWorkAttribute)
    {
        //方法定义了UnitOfWorkAttribute
        var attr = methodInfo.GetCustomAttribute<UnitOfWorkAttribute>(true);
        if (attr != null)
        {
            unitOfWorkAttribute = attr;
            return attr.IsEnabled;
        }

        unitOfWorkAttribute = null;
        return false;
    }
}