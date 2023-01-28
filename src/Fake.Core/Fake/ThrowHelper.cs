using System.Diagnostics;

namespace Fake;

[DebuggerStepThrough]
public static class ThrowHelper
{
    [ContractAnnotation("value:null => halt")]
    public static T ThrowIfNull<T>(
        T value,
        [InvokerParameterName] [CanBeNull] string parameterName = null,
        [CanBeNull] string message = null)
    {
        if (value != null) return value;
        throw new ArgumentNullException(parameterName, message);
    }
}