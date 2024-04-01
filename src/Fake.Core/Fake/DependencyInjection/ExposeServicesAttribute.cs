using System.Diagnostics;
using System.Reflection;

namespace Fake.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ExposeServicesAttribute(params Type[] exposedServiceTypes) : Attribute, IExposedServiceTypesProvider
{
    /// <summary>
    /// 暴露按照约定命名的接口
    /// </summary>
    /// <remarks>
    /// <para>
    /// 命名约定：接口名去掉I匹配目标类型结尾的接口
    /// 例如A:IA2,IA，那么暴露IA
    /// 例如CustomA:ICustomA,IA，那么暴露ICustomA和IA
    /// </para>
    /// </remarks>
    public bool ExposeInterface { get; set; } = true;

    /// <summary>
    /// 暴露自身
    /// </summary>
    public bool ExposeSelf { get; set; } = true;


    public ServiceIdentifier[] GetExposedServices(Type implementType)
    {
        var res = new List<Type>(exposedServiceTypes);
        if (ExposeInterface)
        {
            foreach (var type in GetConventionalNamingServiceTypes(implementType))
            {
                res.TryAdd(type);
            }
        }

        if (ExposeSelf)
        {
            res.TryAdd(implementType);
        }

        return res.Select(t => new ServiceIdentifier(t)).ToArray();
    }

    private static List<Type> GetConventionalNamingServiceTypes(Type targetType)
    {
        var serviceTypes = new List<Type>();

        Type[] interfaces = targetType.GetTypeInfo().GetInterfaces();

        foreach (var @interface in interfaces)
        {
            Debug.Assert(@interface != null, $"{nameof(@interface)} != null");

            if (@interface is null) throw new ArgumentNullException(nameof(@interface));

            var interfaceName = @interface.Name.AsSpan();
            if (@interface.IsGenericType)
            {
                interfaceName = interfaceName.Slice(0, interfaceName.IndexOf('`'));
            }

            if (interfaceName[0] == 'I')
            {
                interfaceName = interfaceName.Slice(1, interfaceName.Length - 1);
            }

            //TODO：为什么这里暴露了泛型，仍然获取不了服务
            // if (implementType.IsGenericType)
            // {
            //     if (implementType.Name.Substring(0, implementType.Name.IndexOf('`')).EndsWith(interfaceName.ToString()))
            //     {
            //         serviceTypes.Add(@interface);
            //         continue;
            //     }
            // }

            if (targetType.Name.EndsWith(interfaceName.ToString()))
            {
                serviceTypes.Add(@interface);
            }
        }

        return serviceTypes;
    }
}