using Fake;

namespace System.Collections.Generic;

public static class FakeCollectionExtensions
{
    /// <summary>
    /// 检查给定的集合对象是否为null或没有元素。
    /// </summary>
    public static bool IsNullOrEmpty<T>(this ICollection<T> source)
    {
        return source is not { Count: > 0 };
    }

    /// <summary>
    /// 尝试添加项到集合，项不存在则成功添加并返回true，否则返回false
    /// </summary>
    /// <param name="source"></param>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool TryAdd<T>(this ICollection<T> source, T item)
    {
        ThrowHelper.ThrowIfNull(source, nameof(source));

        if (source.Contains(item))
        {
            return false;
        }

        source.Add(item);
        return true;
    }
}