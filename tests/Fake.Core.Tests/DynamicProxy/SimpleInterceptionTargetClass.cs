using Fake.Castle.DynamicProxy;
using Fake.DependencyInjection;
using Fake.Logging;

namespace Fake.Core.Tests.DynamicProxy;

[FakeIntercept(typeof(SimpleAsyncInterceptor))]
[ExposeServices(typeof(ISimpleInterceptionTarget), ExposeSelf = true)]
public class SimpleInterceptionTargetClass : ICanLog, ITransientDependency, ISimpleInterceptionTarget
{
    public List<string> Logs { get; } = new();

    public virtual async Task DoItAsync()
    {
        Logs.Add("EnterDoItAsync");
        await Task.Delay(5);
        Logs.Add("MiddleDoItAsync");
        await Task.Delay(5);
        Logs.Add("ExitDoItAsync");
    }
}