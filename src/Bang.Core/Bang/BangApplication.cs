using System.Reflection;
using Bang.Extensions;
using Bang.Logging;
using Bang.Modularity;
using Microsoft.Extensions.Logging;

namespace Bang;

public abstract class BangApplication : IBangApplicationInfo
{
    public IReadOnlyList<IModuleDescriptor> Modules { get; }
    public string ApplicationName { get; }
    public string ApplicationId { get; } = Guid.NewGuid().ToString();

    public Type StartupModuleType { get; }
    public IServiceCollection Services { get; }
    public IServiceProvider ServiceProvider { get; private set; }

    private bool _configuredServices;

    internal BangApplication(
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

        Modules = LoadModules(services);

        ConfigureServices();
    }
    
    public void ConfigureServices()
    {
        if (_configuredServices)
        {
            throw new BangInitializationException("Services已经配置过了，不要重复配置");
        }

        var context = new ServiceConfigurationContext(Services);

        // PreConfigureServices
        foreach (var module in Modules)
        {
            try
            {
                module.Instance.PreConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new BangInitializationException(
                    $"模块 {module.Type.AssemblyQualifiedName} 在 {nameof(IConfigureServicesLifecycle.PreConfigureServices)} 阶段发生异常。 有关详细信息，请参阅内部异常。",
                    ex);
            }
        }

        // ConfigureServices
        foreach (var module in Modules)
        {
            try
            {
                module.Instance.ConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new BangInitializationException(
                    $"模块 {module.Type.AssemblyQualifiedName} 在 {nameof(IConfigureServicesLifecycle.ConfigureServices)} 阶段发生异常。 有关详细信息，请参阅内部异常。",
                    ex);
            }
        }

        // PostConfigureServices
        foreach (var module in Modules)
        {
            try
            {
                module.Instance.PostConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new BangInitializationException(
                    $"模块 {module.Type.AssemblyQualifiedName} 在 {nameof(IConfigureServicesLifecycle.PostConfigureServices)} 阶段发生异常。 有关详细信息，请参阅内部异常。",
                    ex);
            }
        }

        _configuredServices = true;
    }
    
    public virtual void Shutdown()
    {
        // Shutdown
        var context = new ApplicationShutdownContext(ServiceProvider);
        foreach (var module in Modules)
        {
            try
            {
                module.Instance.Shutdown(context);
            }
            catch (Exception ex)
            {
                throw new BangInitializationException(
                    $"模块 {module.Type.AssemblyQualifiedName} 在 {nameof(IShutdownLifecycle.Shutdown)} 阶段发生异常。 有关详细信息，请参阅内部异常。",
                    ex);
            }
        }
    }

    public virtual void Dispose()
    {
    }
    
    protected virtual void Configure()
    {
        // log
        var logger = ServiceProvider.GetService<ILogger<BangApplication>>();
        
        var initLogger = ServiceProvider.GetRequiredService<IBangLoggerFactory>().Create<BangApplication>();
        
        foreach (var entry in initLogger.Entries)
        {
            logger.Log(entry.LogLevel, entry.EventId, entry.State, entry.Exception, entry.Formatter);
        }

        initLogger.Entries.Clear();
        
        var context = new ApplicationConfigureContext(ServiceProvider);

        // PreConfigure
        foreach (var module in Modules)
        {
            try
            {
                module.Instance.PreConfigure(context);
            }
            catch (Exception ex)
            {
                throw new BangInitializationException(
                    $"模块 {module.Type.AssemblyQualifiedName} 在 {nameof(IConfigureLifecycle.PreConfigure)} 阶段发生异常。 有关详细信息，请参阅内部异常。",
                    ex);
            }
        }
        
        // Configure
        foreach (var module in Modules)
        {
            try
            {
                module.Instance.Configure(context);
            }
            catch (Exception ex)
            {
                throw new BangInitializationException(
                    $"模块 {module.Type.AssemblyQualifiedName} 在 {nameof(IConfigureLifecycle.Configure)} 阶段发生异常。 有关详细信息，请参阅内部异常。",
                    ex);
            }
        }
        
        // PostConfigure
        foreach (var module in Modules)
        {
            try
            {
                module.Instance.PostConfigure(context);
            }
            catch (Exception ex)
            {
                throw new BangInitializationException(
                    $"模块 {module.Type.AssemblyQualifiedName} 在 {nameof(IConfigureLifecycle.PostConfigure)} 阶段发生异常。 有关详细信息，请参阅内部异常。",
                    ex);
            }
        }
    }
    
    private IReadOnlyList<IModuleDescriptor> LoadModules(IServiceCollection services)
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
}