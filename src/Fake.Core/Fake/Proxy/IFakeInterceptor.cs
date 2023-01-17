using System.Reflection;

namespace Fake.Proxy;

public interface IFakeInterceptor
{
    Task InterceptAsync(IFakeMethodInvocation invocation);
}

public interface IFakeMethodInvocation
{
    object[] Arguments { get; }
    IReadOnlyDictionary<string, object> ArgumentsDictionary { get; }
    Type[] GenericArguments { get; }
    object TargetObject { get; }
    MethodInfo Method { get; }
    object ReturnValue { get; }
    Task ProcessAsync();
}