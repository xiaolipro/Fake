namespace Fake.DependencyInjection;

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
    /// <param name="implementType"></param>
    /// <returns></returns>
    ServiceIdentifier[] GetExposedServices(Type implementType);
}