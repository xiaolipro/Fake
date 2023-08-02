using System.Reflection;

namespace Fake.Collections;

public interface ITypeList<in TBaseType> : IList<Type>
{
    /// <summary>
    /// 添加类型到列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void Add<T>() where T : TBaseType;
    
    /// <summary>
    /// 添加类型枚举器到列表
    /// </summary>
    /// <param name="types">类型枚举器</param>
    void AddRange(IEnumerable<TypeInfo> types);
    
    /// <summary>
    /// 尝试添加类型到列表，如果已存在则直接返回false，否则添加并返回true
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    bool TryAdd<T>()  where T : TBaseType;
    
    /// <summary>
    /// 检查类型是否存在列表中
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <returns></returns>
    bool Contains<T>() where T : TBaseType;

    /// <summary>
    /// 从列表中移除指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void Remove<T>() where T : TBaseType;
}