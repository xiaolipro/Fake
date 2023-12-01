using Microsoft.Extensions.Configuration;

namespace Fake;

public class FakeApplicationCreationOptions
{
    [CanBeNull] public string ApplicationName { get; set; }


    public IServiceCollection Services { get; }

    /// <summary>
    /// 仅在未注册IConfiguration时生效
    /// </summary>

    public FakeConfigurationBuilderOptions? Configuration { get; }


    public FakeApplicationCreationOptions(IServiceCollection services)
    {
        Services = ThrowHelper.ThrowIfNull(services, nameof(services));
        Configuration = new FakeConfigurationBuilderOptions();
    }
}