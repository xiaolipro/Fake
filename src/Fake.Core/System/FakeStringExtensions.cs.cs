using Fake;

namespace System;

public static class FakeStringExtensions
{
    /// <summary>
    /// 表示此字符串 是 null或空字符串。
    /// </summary>
    public static bool IsNullOrEmpty(this string? str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 表示此字符串 不是 null或空字符串。
    /// </summary>
    public static bool NotBeNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 表示此字符串 是 null或空字符串或空白格。
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string? str)
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
    public static bool In(this string str, IEnumerable<string>? list)
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
    public static string EndsWithAppend(this string str, string end,
        StringComparison comparisonType = StringComparison.Ordinal)
    {
        ThrowHelper.ThrowIfNull(str, nameof(str));
        ThrowHelper.ThrowIfNull(end, nameof(end));

        if (str.EndsWith(end, comparisonType)) return str;

        return str + end;
    }

    /// <summary>
    /// 如果str不是以start开始，则在句首追加start
    /// </summary>
    /// <param name="str"></param>
    /// <param name="start"></param>
    /// <param name="comparisonType"></param>
    /// <returns>追加后的结果</returns>
    /// <exception cref="ArgumentNullException">str is null</exception>
    public static string StartsWithAppend(this string str, string start,
        StringComparison comparisonType = StringComparison.Ordinal)
    {
        ThrowHelper.ThrowIfNull(str, nameof(str));
        ThrowHelper.ThrowIfNull(start, nameof(start));

        if (str.StartsWith(start, comparisonType)) return str;

        return start + str;
    }

    /// <summary>
    /// 截取给定长度子串，如果给定长度超出原字符串，则返回原字符串
    /// </summary>
    /// <param name="str">原字符串</param>
    /// <param name="len">给定长度</param>
    /// <returns></returns>
    public static string? Truncate(this string? str, int len)
    {
        if (str.IsNullOrEmpty()) return str;

        if (str!.Length < len) len = str.Length;

        return str.Substring(0, len);
    }


    /// <summary>
    /// 截取给定长度子串，如果给定长度超出原字符串，则返回原字符串并用suffix替代超出部分
    /// </summary>
    /// <param name="str">原字符串</param>
    /// <param name="maxLen">给定长度</param>
    /// <param name="suffix">后缀</param>
    /// <returns></returns>
    public static string? TruncateWithSuffix(this string? str, int maxLen = 64, string suffix = "...")
    {
        if (str.IsNullOrEmpty()) return str;

        if (maxLen - suffix.Length <= 0) return str.Truncate(maxLen);

        if (str!.Length <= maxLen) return str;
        return str.Truncate(maxLen - suffix.Length) + suffix;
    }
}