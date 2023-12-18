using Fake.Castle.DynamicProxy;

namespace Fake.DynamicProxy;

[FakeIntercept(typeof(SimpleAsyncInterceptor))]
public interface ISimpleInterceptionTarget
{
    public List<string> Logs { get; }
    public Task DoItAsync();
}