using Fake.Modularity;
using Microsoft.Extensions.Configuration;

namespace Fake;

public interface IFakeApplication : IServiceProviderAccessor, IModuleContainer, IApplicationInfo, IDisposable
{
    /// <summary>
    /// 启动模块的类型
    /// </summary>
    Type StartupModuleType { get; }

    /// <summary>
    /// 程序配置
    /// </summary>
    IConfiguration Configuration { get; }

    /// <summary>
    /// 服务容器。应用程序初始化后，无法将新服务添加到此容器。
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// 调用模块的Pre/Post/ConfigureServicesAsync方法
    /// </summary>
    void ConfigureServices();

    /// <summary>
    /// 程序关闭
    /// </summary>
    void Shutdown();
}