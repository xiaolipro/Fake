using System;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Testing;

public abstract class TestServiceProviderAccessor
{
    protected IServiceProvider ServiceProvider { get; set; } = default!;

    protected T? GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }

    protected T GetRequiredService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }
}