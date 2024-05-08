using System.Text;
using System.Text.RegularExpressions;
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
    public static string EnsureEndsWith(this string str, string end,
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
    public static string EnsureStartWith(this string str, string start,
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

    public static byte[] ToBytes(this string str, Encoding encoding)
    {
        ThrowHelper.ThrowIfNull(str, nameof(str));
        ThrowHelper.ThrowIfNull(encoding, nameof(encoding));

        return encoding.GetBytes(str);
    }

    /// <summary>
    /// Gets a substring of a string from end of the string.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
    public static string Right(this string str, int len)
    {
        ThrowHelper.ThrowIfNull(str, nameof(str));

        if (str.Length < len)
        {
            throw new ArgumentException("len argument can not be bigger than given string's length!");
        }

        return str.Substring(str.Length - len, len);
    }

    /// <summary>
    /// Gets a substring of a string from beginning of the string.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
    public static string Left(this string str, int len)
    {
        ThrowHelper.ThrowIfNull(str, nameof(str));

        if (str.Length < len)
        {
            throw new ArgumentException("len argument can not be bigger than given string's length!");
        }

        return str.Substring(0, len);
    }

    public static string RemovePostfix(this string str, params string[] postFixes)
    {
        return str.RemovePostfix(StringComparison.Ordinal, postFixes);
    }

    public static string RemovePostfix(this string str, StringComparison comparisonType, params string[] postFixes)
    {
        if (str.IsNullOrEmpty())
        {
            return str;
        }

        if (postFixes.IsNullOrEmpty())
        {
            return str;
        }

        foreach (var postFix in postFixes)
        {
            if (str.EndsWith(postFix, comparisonType))
            {
                return str.Left(str.Length - postFix.Length);
            }
        }

        return str;
    }

    /// <summary>
    /// Removes first occurrence of the given prefixes from beginning of the given string.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="preFixes">one or more prefix.</param>
    /// <returns>Modified string or the same string if it has not any of given prefixes</returns>
    public static string RemovePrefix(this string str, params string[] preFixes)
    {
        return str.RemovePrefix(StringComparison.Ordinal, preFixes);
    }

    /// <summary>
    /// Removes first occurrence of the given prefixes from beginning of the given string.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="comparisonType">String comparison type</param>
    /// <param name="preFixes">one or more prefix.</param>
    /// <returns>Modified string or the same string if it has not any of given prefixes</returns>
    public static string RemovePrefix(this string str, StringComparison comparisonType, params string[] preFixes)
    {
        if (str.IsNullOrEmpty())
        {
            return str;
        }

        if (preFixes.IsNullOrEmpty())
        {
            return str;
        }

        foreach (var preFix in preFixes)
        {
            if (str.StartsWith(preFix, comparisonType))
            {
                return str.Right(str.Length - preFix.Length);
            }
        }

        return str;
    }

    /// <summary>
    /// Converts PascalCase string to camelCase string.
    /// </summary>
    /// <param name="str">String to convert</param>
    /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
    /// <param name="handleAbbreviations">set true to if you want to convert 'XYZ' to 'xyz'.</param>
    /// <returns>camelCase of the string</returns>
    public static string ToCamelCase(this string str, bool useCurrentCulture = false, bool handleAbbreviations = false)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        if (str.Length == 1)
        {
            return useCurrentCulture ? str.ToLower() : str.ToLowerInvariant();
        }

        if (handleAbbreviations && !str.Any(c => char.IsLetter(c) && char.IsLower(c)))
        {
            return useCurrentCulture ? str.ToLower() : str.ToLowerInvariant();
        }

        return (useCurrentCulture ? char.ToLower(str[0]) : char.ToLowerInvariant(str[0])) + str.Substring(1);
    }

    /// <summary>
    /// Converts given PascalCase/camelCase string to kebab-case.
    /// </summary>
    /// <param name="str">String to convert.</param>
    /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
    public static string ToKebabCase(this string str, bool useCurrentCulture = false)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        str = str.ToCamelCase();

        return useCurrentCulture
            ? Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + "-" + char.ToLower(m.Value[1]))
            : Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + "-" + char.ToLowerInvariant(m.Value[1]));
    }
}