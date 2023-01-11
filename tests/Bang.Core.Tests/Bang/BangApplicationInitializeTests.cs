using Bang.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Bang;

public class BangApplicationInitializeTests
{
    [Fact]
    void 初始化一个模块()
    {
        using (var app = BangApplicationFactory.Create<IndependentModule>())
        {
            var module = app.Services.GetSingletonInstance<IndependentModule>();
            
            // Action
            app.Configure();
            // Assert
            app.ServiceProvider.GetRequiredService<IndependentModule>().ShouldBeSameAs(module);
        }
    }
}