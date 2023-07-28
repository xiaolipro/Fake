namespace Fake.Modularity;

public interface IFakeModule : IConfigureServicesLifecycle, IConfigureApplicationLifecycle, IShutdownLifecycle
{
    /// <summary>
    /// 此模块跳过Fake提供的自动服务注册
    /// </summary>
    public bool SkipServiceRegistration { get; }
}