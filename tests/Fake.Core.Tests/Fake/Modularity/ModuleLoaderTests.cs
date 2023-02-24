using Fake.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.Modularity;

public class ModuleLoaderTests
{
    [Fact]
    void 应该按照依赖顺序加载模块()
    {
        var loader = new FakeModuleLoader();
        var services = new ServiceCollection();
        services.AddSingleton<IInitLoggerFactory>(new FakeInitLoggerFactory());
        var modules = loader.LoadModules(services, typeof(StartupModuleApplication));
        
        modules.Length.ShouldBe(3);
        modules[0].Type.ShouldBe(typeof(FakeCoreModuleApplication));
        modules[1].Type.ShouldBe(typeof(CustomModuleApplication));
        modules[2].Type.ShouldBe(typeof(StartupModuleApplication));
    }
    
    [Fact]
    void 没有注册日志应该抛异常()
    {
        Should.Throw<InvalidOperationException>(() =>
        {
            var loader = new FakeModuleLoader();
            var services = new ServiceCollection();
            loader.LoadModules(services, typeof(StartupModuleApplication));
        });
    }
}

[DependsOn(typeof(CustomModuleApplication))]
public class StartupModuleApplication : FakeModuleApplication
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}

public class CustomModuleApplication : FakeModuleApplication
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}