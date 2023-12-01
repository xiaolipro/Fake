using Fake;

namespace System;

public static class FakeTypeExtensions
{
    /// <summary>
    /// 是否可以分配给 <typeparamref name="TTarget"></typeparamref>。
    /// 内部使用 <see cref="Type.IsAssignableFrom"/>，翻转实现
    /// </summary>
    /// <typeparam name="TTarget">目标类型</typeparam>
    public static bool IsAssignableTo<TTarget>(this Type fromType)
    {
        ThrowHelper.ThrowIfNull(fromType, nameof(fromType));

        return fromType.IsAssignableTo(typeof(TTarget));
    }

    /// <summary>
    /// 是否可以分配给<paramref name="targetType"></paramref>，翻转 <see cref="Type.IsAssignableFrom"/>。
    /// </summary>
    /// <param name="fromType">可分配类型</param>
    /// <param name="targetType">目标类型</param>
    /// <returns>A:IA 则 typeof(A).IsAssignableTo(typeof(IA)) = true</returns>
    public static bool IsAssignableTo(this Type fromType, Type targetType)
    {
        ThrowHelper.ThrowIfNull(targetType, nameof(targetType));
        ThrowHelper.ThrowIfNull(fromType, nameof(fromType));

        return targetType.IsAssignableFrom(fromType);
    }

    /// <summary>
    /// 获取类型约定名称
    /// 约定如下：
    /// 1.非泛型直接返回Name
    /// 2.泛型参数以separator拼接
    /// </summary>
    /// <param name="type"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string GetName(this Type type, string separator = ",")
    {
        // 不是泛型直接返回
        if (!type.IsGenericType) return type.Name;

        var genericTypes = string.Join(separator, type.GetGenericArguments().Select(x => x.Name).ToArray());

        // <A,B>
        return $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
    }
}