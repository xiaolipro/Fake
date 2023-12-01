using System.Diagnostics;

namespace Fake;

[DebuggerStepThrough]
public static class ThrowHelper
{
    public static T ThrowIfNull<T>(
        T value,
        string? parameterName = null,
        string? message = null)
    {
        if (value != null) return value;
        throw new ArgumentNullException(parameterName, message);
    }

    public static string? ThrowIfNullOrWhiteSpace(
        string? value,
        string? parameterName = null)
    {
        if (value.IsNotNullOrWhiteSpace()) return value;
        throw new ArgumentException($"{parameterName}不能是null，empty或white space", parameterName);
    }
}