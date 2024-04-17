using Fake.Core.Tests.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Core.Tests;

public class FakeApplicationInitializeTests
{
    [Fact]
    void FakeApplication使用流程()
    {
        using var app = FakeApplicationFactory.Create<IndependentModule>();

        // Arrange
        var module = app.Services.GetInstance<IndependentModule>();

        // Action
        app.InitializeApplication();

        // Assert
        app.ServiceProvider.GetRequiredService<IndependentModule>().ShouldBeSameAs(module);

        // Action
        app.Shutdown();
    }

    [Fact]
    void 模块初始化前不能使用ServiceProvider()
    {
        Should.Throw<FakeException>(() =>
        {
            using var app = FakeApplicationFactory.Create<IndependentModule>();
            app.ServiceProvider.GetRequiredService<IndependentModule>();
        });
    }
}