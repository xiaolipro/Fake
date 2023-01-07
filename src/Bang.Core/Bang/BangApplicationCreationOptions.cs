using Microsoft.Extensions.Configuration;

namespace Bang;

public class BangApplicationCreationOptions
{
    [CanBeNull]
    public string ApplicationName { get; set; }
    
    [NotNull]
    public IServiceCollection Services { get; }
    
    /// <summary>
    /// The options in this property only take effect when IConfiguration not registered.
    /// </summary>
    [NotNull]
    public BangConfigurationBuilderOptions Configuration { get; }


    public BangApplicationCreationOptions([NotNull] IServiceCollection services)
    {
        Services = ThrowHelper.ThrowIfNull(services, nameof(services));
        Configuration = new BangConfigurationBuilderOptions();
    }
}