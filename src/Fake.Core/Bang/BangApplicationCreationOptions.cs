using Microsoft.Extensions.Configuration;

namespace Fake;

public class FakeApplicationCreationOptions
{
    [CanBeNull]
    public string ApplicationName { get; set; }
    
    [NotNull]
    public IServiceCollection Services { get; }
    
    /// <summary>
    /// The options in this property only take effect when IConfiguration not registered.
    /// </summary>
    [NotNull]
    public FakeConfigurationBuilderOptions Configuration { get; }


    public FakeApplicationCreationOptions([NotNull] IServiceCollection services)
    {
        Services = ThrowHelper.ThrowIfNull(services, nameof(services));
        Configuration = new FakeConfigurationBuilderOptions();
    }
}