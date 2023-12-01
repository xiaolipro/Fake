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
    public static T? As<T>(this object obj) where T : class
    {
        return obj as T;
    }

    /// <summary>
    /// 将obj强转为给定类型T，等价于obj is T
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Is<T>(this object obj)
        where T : class
    {
        return (T)obj;
    }
}