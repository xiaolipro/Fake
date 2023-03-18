using Fake.Modularity;
using Fake.Threading;
using Fake.Timing;

/// <summary>
/// 核心模块
/// <remarks>自动载入，无须依赖</remarks>
/// </summary>
public class FakeCoreModule : FakeModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IClock, Clock>();
        context.Services.AddTransient(typeof(IAmbientScopeProvider<>), typeof(AmbientScopeProvider<>));
    }
}