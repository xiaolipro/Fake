using System.Diagnostics;
using Mapster;

namespace Fake.ObjectMapping.Mapster.Profiles;

public interface IProfile
{
    /// <summary>
    /// 创建映射配置器,如果已经存在类型映射则覆盖
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TSrc"></typeparam>
    /// <returns></returns>
    TypeAdapterSetter<TSource, TSrc> CreateNewConfig<TSource, TSrc>();

    /// <summary>
    /// 指定类型映射配置不存在，那它将创建一个新的映射，如果指定类型的映射配置已存在，那么它将会扩展已有的映射配置
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TSrc"></typeparam>
    /// <returns></returns>
    TypeAdapterSetter<TSource, TSrc> CreateForType<TSource, TSrc>();
}