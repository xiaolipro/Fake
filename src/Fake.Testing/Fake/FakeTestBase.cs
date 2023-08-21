using System;
using Microsoft.Extensions.DependencyInjection;

namespace Fake;

public abstract class FakeTestBase : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; protected set; }

    protected T GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }
    
    protected T GetRequiredService<T>()
    {
        return ServiceProvider.GetRequiredService<T>();
    }
}