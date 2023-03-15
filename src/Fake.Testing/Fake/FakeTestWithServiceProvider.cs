using System;
using Microsoft.Extensions.DependencyInjection;

namespace Fake;

public abstract class FakeTestWithServiceProvider
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