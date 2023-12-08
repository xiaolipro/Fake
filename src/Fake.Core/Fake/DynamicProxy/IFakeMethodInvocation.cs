using System.Reflection;

namespace Fake.DynamicProxy;

public interface IFakeMethodInvocation
{
    object[] Arguments { get; }
    IReadOnlyDictionary<string, object> ArgumentsDictionary { get; }
    Type[] GenericArguments { get; }
    object TargetObject { get; }
    MethodInfo Method { get; }
    object? ReturnValue { get; set; }
    Task ProcessAsync();
}