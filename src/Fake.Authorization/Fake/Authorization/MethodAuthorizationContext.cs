using System.Reflection;

namespace Fake.Authorization;

public class MethodAuthorizationContext(MethodInfo method)
{
    public MethodInfo Method { get; } = method;
}