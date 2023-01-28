using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Autofac;

public class FakeAutofacServiceProviderFactory: IServiceProviderFactory<ContainerBuilder>
{
    private readonly ContainerBuilder _builder;
    private IServiceCollection _services;

    public FakeAutofacServiceProviderFactory(ContainerBuilder builder)
    {
        _builder = builder;
    }
    
    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        _services = services;

        _builder.Populate(services);

        return _builder;
    }

    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
    {
        ThrowHelper.ThrowIfNull(containerBuilder, nameof(containerBuilder));

        return new AutofacServiceProvider(containerBuilder.Build());
    }
}