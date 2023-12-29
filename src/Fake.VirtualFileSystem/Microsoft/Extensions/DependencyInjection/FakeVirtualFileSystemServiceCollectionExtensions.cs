using System;
using Fake.Modularity;
using Fake.VirtualFileSystem;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeVirtualFileSystemServiceCollectionExtensions
{
    public static IServiceCollection AddFakeVirtualFileSystem<TFakeModule>(this IServiceCollection services,
        string? root = null)
        where TFakeModule : FakeModule
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.Configure<FakeVirtualFileSystemOptions>(options => { options.FileProviders.Add<TFakeModule>(root); });

        return services;
    }
}