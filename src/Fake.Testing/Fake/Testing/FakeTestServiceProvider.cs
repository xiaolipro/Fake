using System;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Testing;

public abstract class FakeTestServiceProvider : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; protected set; } = null!;

    protected T? GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }

    protected T GetRequiredService<T>()
    {
        return ServiceProvider.GetRequiredService<T>();
    }
}