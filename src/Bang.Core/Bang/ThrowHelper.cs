﻿using System.Diagnostics;

namespace Bang;

[DebuggerStepThrough]
public static class ThrowHelper
{
    [ContractAnnotation("value:null => halt")]
    public static T ThrowIfNull<T>(
        T value,
        [InvokerParameterName] [CanBeNull] string parameterName = null)
    {
        if (value != null) return value;
        throw new ArgumentNullException(parameterName);
    }
}