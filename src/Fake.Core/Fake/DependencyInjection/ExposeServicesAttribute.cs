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
    public bool ExposeConventionalInterfaces { get; set; }

    public bool ExposeSelf { get; set; }

    public ExposeServicesAttribute(params Type[] exposedServiceTypes)
    {
        ExposedServiceTypes = exposedServiceTypes ?? Type.EmptyTypes;
    }

    
    public Type[] GetExposedServiceTypes(Type targetType)
    {
        var exposedServiceTypes = ExposedServiceTypes.ToList();

        if (ExposeSelf)
        {
            exposedServiceTypes.TryAdd(targetType);
        }

        if (ExposeConventionalInterfaces)
        {
            foreach (var type in GetConventionalNamingServiceTypes(targetType))
            {
                exposedServiceTypes.TryAdd(type);
            }
        }

        return exposedServiceTypes.ToArray();
    }

    private static List<Type> GetConventionalNamingServiceTypes(Type targetType)
    {
        var serviceTypes = new List<Type>();

        var interfaces = targetType.GetTypeInfo().GetInterfaces();

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

            if (targetType.Name.EndsWith(interfaceName.ToString()))
            {
                serviceTypes.Add(@interface);
            }
        }

        return serviceTypes;
    }
}

/// <summary>
/// 暴露服务类型的供应商
/// </summary>
/// <remarks>
/// 你完全可以自己写一个attribute实现IExposedServiceTypesProvider，然后自定义自己的暴露策略
/// </remarks>
public interface IExposedServiceTypesProvider
{
    /// <summary>
    /// 获取需要暴露的服务
    /// </summary>
    /// <param name="targetType"></param>
    /// <returns></returns>
    Type[] GetExposedServiceTypes(Type targetType);
}