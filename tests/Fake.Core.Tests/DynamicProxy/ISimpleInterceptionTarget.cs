namespace Fake.Core.Tests.DynamicProxy;

public interface ISimpleInterceptionTarget
{
    public List<string> Logs { get; }
    public Task DoItAsync();
}