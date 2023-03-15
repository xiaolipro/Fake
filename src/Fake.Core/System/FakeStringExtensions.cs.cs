using Fake;

namespace System;

public static class FakeStringExtensions
{
    /// <summary>
    /// 表示此字符串 是 null或空字符串。
    /// </summary>
    [ContractAnnotation("str:null => true")]
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 表示此字符串 不是 null或空字符串。
    /// </summary>
    [ContractAnnotation("str:null => true")]
    public static bool NotBeNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 表示此字符串 是 null或空字符串或空白格。
    /// </summary>
    [ContractAnnotation("str:null => true")]
    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// 表示此字符串 不是 null或空字符串或空白格。
    /// </summary>
    [ContractAnnotation("str:null => false")]
    public static bool NotBeNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// 表示字符串 存在于 该<see cref="Enumerable"/>实例中，
    /// 内部使用Enumerable.Contains。
    /// </summary>
    /// <param name="str"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool In(this string str, IEnumerable<string> list)
    {
        if (list == null) return false;
        return list.Contains(str);
    }

    /// <summary>
    /// 如果str不是以end结尾，则在句尾追加end
    /// </summary>
    /// <param name="str"></param>
    /// <param name="end"></param>
    /// <param name="comparisonType"></param>
    /// <returns>追加后的结果</returns>
    /// <exception cref="ArgumentNullException">str is null</exception>
    public static string EndsWithOrAppend([NotNull] this string str, [NotNull] string end,
        StringComparison comparisonType = StringComparison.Ordinal)
    {
        ThrowHelper.ThrowIfNull(str, nameof(str));
        ThrowHelper.ThrowIfNull(end, nameof(end));

        if (str.EndsWith(end, comparisonType)) return str;

        return str + end;
    }

    public static bool NotBeEndsWith([NotNull] this string str, [NotNull] string start,
        StringComparison comparisonType = StringComparison.Ordinal)
    {
        ThrowHelper.ThrowIfNull(str, nameof(str));
        ThrowHelper.ThrowIfNull(start, nameof(start));

        return !str.EndsWith(start, comparisonType);
    }

    /// <summary>
    /// 如果str不是以start开始，则在句首追加start
    /// </summary>
    /// <param name="str"></param>
    /// <param name="start"></param>
    /// <param name="comparisonType"></param>
    /// <returns>追加后的结果</returns>
    /// <exception cref="ArgumentNullException">str is null</exception>
    public static string StartsWithOrAppend([NotNull] this string str, [NotNull] string start,
        StringComparison comparisonType = StringComparison.Ordinal)
    {
        ThrowHelper.ThrowIfNull(str, nameof(str));
        ThrowHelper.ThrowIfNull(start, nameof(start));

        if (str.StartsWith(start, comparisonType)) return str;

        return start + str;
    }


    public static bool NotBeStartsWith([NotNull] this string str, [NotNull] string start,
        StringComparison comparisonType = StringComparison.Ordinal)
    {
        ThrowHelper.ThrowIfNull(str, nameof(str));
        ThrowHelper.ThrowIfNull(start, nameof(start));

        return !str.StartsWith(start, comparisonType);
    }
}