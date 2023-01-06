using System.Reflection;
using Bang.Extensions;
using Bang.Modularity;

namespace Bang;

public class BangApplicationInfo:IBangApplicationInfo
{
    public IReadOnlyList<IModuleDescriptor> Modules { get; }
    public string ApplicationName { get; }
    public string ApplicationId { get; }


    public Type StartupModuleType { get; }
    public IServiceCollection Services { get; }
    public IServiceProvider ServiceProvider { get; private set; }

    private bool _configuredServices;
    
    internal BangApplicationInfo(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<BangApplicationCreationOptions> optionsAction)
    {
        ThrowHelper.NotNull(startupModuleType, nameof(startupModuleType));
        ThrowHelper.NotNull(services, nameof(services));

        StartupModuleType = startupModuleType;
        Services = services;

        services.TryAddObjectAccessor<IServiceProvider>();

        var options = new BangApplicationCreationOptions(services);
        optionsAction?.Invoke(options);

        ApplicationName = GetApplicationName(options);

        services.AddSingleton<IBangApplicationInfo>(this);
        services.AddSingleton<IApplicationInfo>(this);
        services.AddSingleton<IModuleContainer>(this);

        services.AddLogging();
        services.AddBangCoreServices(this, options);

        Modules = InitializeModules(services);
    }

    private IReadOnlyList<IModuleDescriptor> InitializeModules(IServiceCollection services)
    {
        return services.GetSingletonInstance<IModuleLoader>().LoadModules(services, StartupModuleType);
    }
    
    private static string GetApplicationName(BangApplicationCreationOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.ApplicationName))
        {
            return options.ApplicationName;
        }

        var configuration = options.Services.GetConfigurationOrDefault();
        
        if (configuration == default) return Assembly.GetEntryAssembly()?.GetName().Name;
        
        return configuration[nameof(IApplicationInfo.ApplicationName)];
    }
    
    public virtual void ConfigureServices()
    {
        if (_configuredServices)
        {
            throw new BangInitializationException("重复配置IServiceCollection");
        }
        
        var context = new ServiceConfigurationContext(Services);
        Services.AddSingleton(context);
        _configuredServices = true;
    }

    public void Shutdown()
    {
        throw new NotImplementedException();
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}