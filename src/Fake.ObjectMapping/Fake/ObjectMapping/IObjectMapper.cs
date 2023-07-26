namespace Fake.ObjectMapping;

/// <summary>
/// 全局对象映射器
/// </summary>
public interface IObjectMapper
{
    public IAutoMappingProvider AutoMappingProvider { get; }

    /// <summary>
    /// 将<paramref name="source"/>映射成<typeparamref name="TDestination"/>
    /// </summary>
    /// <param name="source">映射源</param>
    /// <typeparam name="TSource">原类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <returns></returns>
    public TDestination Map<TSource, TDestination>(object source);

    /// <summary>
    /// 将<paramref name="source"/>映射成<paramref name="destination"/>的类型.
    /// </summary>
    /// <param name="source">映射源</param>
    /// <param name="destination">目标实例</param>
    /// <typeparam name="TSource">原类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <returns></returns>
    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}