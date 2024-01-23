using System;
using Fake.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Testing;

public abstract class ApplicationTest<TStartupModule> : TestServiceProviderAccessor, IDisposable
    where TStartupModule : IFakeModule
{
    protected IFakeApplication Application { get; set; } = null!;

    protected ApplicationTest()
    {
        InitApplication();
    }

    private void InitApplication()
    {
        var services = CreateServiceCollection();

        BeforeAddFakeApplication(services);
        var application = services.AddFakeApplication<TStartupModule>(SetApplicationCreationOptions);
        Application = application;
        AfterAddFakeApplication(services);

        ServiceProvider = CreateServiceProvider(services);
        application.InitializeApplication(ServiceProvider);
    }

    protected virtual IServiceCollection CreateServiceCollection()
    {
        return new ServiceCollection();
    }

    protected virtual void BeforeAddFakeApplication(IServiceCollection services)
    {
    }

    protected virtual void AfterAddFakeApplication(IServiceCollection services)
    {
    }

    protected virtual void SetApplicationCreationOptions(FakeApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected virtual IServiceProvider CreateServiceProvider(IServiceCollection services)
    {
        return services.BuildServiceProviderFromFactory().CreateScope().ServiceProvider;
    }

    public void Dispose()
    {
        Application.Shutdown();
        Application.Dispose();
    }
}