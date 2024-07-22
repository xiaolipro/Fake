using System.Diagnostics;

namespace Fake;

// todo: 多语言
[DebuggerStepThrough]
public static class ThrowHelper
{
    public static T ThrowIfNull<T>(
        T? value,
        string? parameterName = null,
        string? message = null)
    {
        if (value != null) return value;
        throw new ArgumentNullException(parameterName, message);
    }

    public static string ThrowIfNullOrWhiteSpace(
        string? value,
        string? parameterName = null)
    {
        if (!value.IsNullOrWhiteSpace()) return value!;
        throw new ArgumentException($"{parameterName}不能是null，empty或white space", parameterName);
    }

    public static ICollection<T> ThrowIfNullOrEmpty<T>(ICollection<T>? value,
        string parameterName)
    {
        if (value == null || value.Count <= 0)
        {
            throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
        }

        return value;
    }
}