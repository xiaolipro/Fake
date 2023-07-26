namespace Fake.ObjectMapping;

/// <summary>
/// 实现此接口以覆盖特定类型间的映射
/// </summary>
/// <typeparam name="TSource">源类型</typeparam>
/// <typeparam name="TDestination">目标类型</typeparam>
public interface ISpecificObjectMapper<in TSource, TDestination>
{
    public TDestination Map(object source);
    
    TDestination Map(TSource source, TDestination destination);
}