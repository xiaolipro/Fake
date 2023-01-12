using Bang;

namespace System;

public static class BangTypeExtensions
{
    /// <summary>
    /// 是否可以分配给 <typeparamref name="TTarget"></typeparamref>。
    /// 内部使用 <see cref="Type.IsAssignableFrom"/>，翻转实现
    /// </summary>
    /// <typeparam name="TTarget">目标类型</typeparam>
    public static bool IsAssignableTo<TTarget>([NotNull] this Type fromType)
    {
        ThrowHelper.ThrowIfNull(fromType, nameof(fromType));

        return fromType.IsAssignableTo(typeof(TTarget));
    }
    
    /// <summary>
    /// 是否可以分配给<paramref name="targetType"></paramref>。
    /// 内部使用 <see cref="Type.IsAssignableFrom"/>，翻转实现
    /// </summary>
    /// <param name="targetType">目标类型</param>
    public static bool IsAssignableTo([NotNull] this Type fromType, [NotNull] Type targetType)
    {
        ThrowHelper.ThrowIfNull(targetType, nameof(targetType));
        ThrowHelper.ThrowIfNull(fromType, nameof(fromType));

        return targetType.IsAssignableFrom(fromType);
    }
}