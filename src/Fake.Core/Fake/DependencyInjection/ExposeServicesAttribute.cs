using System.Reflection;

namespace Fake.DependencyInjection;

public class ExposeServicesAttribute : Attribute, IExposedServiceTypesProvider
{
    public Type[] ExposedServiceTypes { get; }

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
    public bool ExposeInterface { get; set; }

    public bool ExposeSelf { get; set; }

    public ExposeServicesAttribute(params Type[] exposedServiceTypes)
    {
        ExposedServiceTypes = exposedServiceTypes ?? Type.EmptyTypes;
    }


    public Type[] GetExposedServiceTypes(Type targetType)
    {
        List<Type> exposedServiceTypes = ExposedServiceTypes.ToList();

        if (ExposeInterface)
        {
            foreach (var type in GetConventionalNamingServiceTypes(targetType))
            {
                exposedServiceTypes.TryAdd(type);
            }
        }

        if (exposedServiceTypes.Count == 0 || ExposeSelf)
        {
            exposedServiceTypes.TryAdd(targetType);
        }

        return exposedServiceTypes.ToArray();
    }

    private static List<Type> GetConventionalNamingServiceTypes(Type targetType)
    {
        var serviceTypes = new List<Type>();

        Type?[] interfaces = targetType.GetTypeInfo().GetInterfaces();

        foreach (var @interface in interfaces)
        {
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
            // if (targetType.IsGenericType)
            // {
            //     if (targetType.Name.Substring(0, targetType.Name.IndexOf('`')).EndsWith(interfaceName.ToString()))
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