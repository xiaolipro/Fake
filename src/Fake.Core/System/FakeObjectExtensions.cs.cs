using System.Runtime.CompilerServices;

namespace System;

public static class FakeObjectExtensions
{
    #region IsIn

    /// <summary>
    /// item是否在给定list中
    /// </summary>
    /// <param name="item"></param>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    public static bool IsIn<T>(this T item, params T[] list)
    {
        return list.Contains(item);
    }

    public static bool IsIn<T>(this T item, IEnumerable<T> items)
    {
        return items.Contains(item);
    }

    #endregion


    /// <summary>
    /// 将obj弱转为给定类型T，等价于obj as T
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [CanBeNull]
    public static T As<T>(this object obj) where T : class
    {
        return obj as T;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Cast<T>(this object obj)
        where T : class
    {
        return (T)obj;
    }
}