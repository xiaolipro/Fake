using System;
using System.Diagnostics.CodeAnalysis;

namespace Fake.Castle.DynamicProxy;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class FakeInterceptAttribute : Attribute
{
    public Type InterceptorType { get; private set; }

    public FakeInterceptAttribute(Type interceptorType)
    {
        InterceptorType = interceptorType;
    }
}