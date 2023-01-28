using Autofac;
using Fake.Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Fake;

public static class FakeApplicationCreationOptionsExtensions
{
    public static void UseAutofac(this FakeApplicationCreationOptions options)
    {
        options.Services.AddAutofacServiceProviderFactory(new ContainerBuilder());
    }

    private static void AddAutofacServiceProviderFactory(this IServiceCollection service,
        ContainerBuilder builder)
    {
        var factory = new FakeAutofacServiceProviderFactory(builder);

        service.AddObjectAccessor(builder);
        service.AddSingleton<IServiceProviderFactory<ContainerBuilder>>(factory);
    }
}