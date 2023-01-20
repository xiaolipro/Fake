using System;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Testing;

public abstract class FakeTest
{
    protected IServiceProvider ServiceProvider { get; set; }

    protected virtual T GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }
    
    protected virtual T GetRequiredService<T>()
    {
        return ServiceProvider.GetRequiredService<T>();
    }
}