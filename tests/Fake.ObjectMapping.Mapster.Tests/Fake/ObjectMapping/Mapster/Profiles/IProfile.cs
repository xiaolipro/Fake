using Mapster;

namespace Fake.ObjectMapping.Mapster.Profiles;

public interface IProfile
{
    /// <summary>
    /// 创建映射配置器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TSrc"></typeparam>
    /// <returns></returns>
    TypeAdapterSetter<TSource, TSrc> CreateConfig<TSource, TSrc>();
}