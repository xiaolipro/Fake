using Fake.DependencyInjection;
using Fake.Logging;

namespace Fake.DynamicProxy;

public interface ISimpleInterceptionTarget
{
    public List<string> Logs { get; }
    public Task DoItAsync();
}

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