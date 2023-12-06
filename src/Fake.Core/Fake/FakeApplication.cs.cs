using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Fake.DependencyInjection;
using Fake.IdGenerators.GuidGenerator;
using Fake.Logging;
using Fake.Modularity;
using Fake.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Fake;

public class FakeApplication : IFakeApplication
{
    public IReadOnlyList<IModuleDescriptor> Modules { get; }
    public string ApplicationName { get; }
    public string ApplicationId { get; } = SimpleGuidGenerator.Instance.GenerateAsString();

    public Type StartupModuleType { get; }

    private IConfiguration _configuration = null!;
    public IConfiguration Configuration => _configuration;
    public IServiceCollection Services { get; }

    private IServiceProvider _serviceProvider = null!;

    public IServiceProvider ServiceProvider
    {
        get
        {
            if (!_initializedModules)
                throw new FakeException($"{nameof(FakeApplication)}初始化前，不能使用{nameof(ServiceProvider)}");
            return _serviceProvider;
        }
    }

    private bool _configuredServices;
    private bool _initializedModules;

    internal FakeApplication(Type startupModuleType, Action<FakeApplicationCreationOptions>? optionsAction)
        : this(startupModuleType, new ServiceCollection(), optionsAction)
    {
    }

    /*
     * AddFakeCoreServices
     * LoadModules
     * ConfigureServices
     */
    internal FakeApplication(
        Type startupModuleType,
        IServiceCollection services,
        Action<FakeApplicationCreationOptions>? optionsAction)
    {
        ThrowHelper.ThrowIfNull(startupModuleType, nameof(startupModuleType));
        ThrowHelper.ThrowIfNull(services, nameof(services));

        StartupModuleType = startupModuleType;
        Services = services;

        // important: 此时的ObjectAccessor<IServiceProvider>只是个空壳，等到InitializeApplication被调用时才真正赋值！
        services.GetOrAddObjectAccessor<IServiceProvider>();

        var options = new FakeApplicationCreationOptions(services);
        optionsAction?.Invoke(options);

        ApplicationName = GetApplicationName(options);

        services.AddSingleton(this);
        services.AddSingleton<IFakeApplication>(this);
        services.AddSingleton<IApplicationInfo>(this);
        services.AddSingleton<IModuleContainer>(this);

        AddFakeCoreServices(services, options);

        Debug.Assert(_configuration != null, "_configuration != null");
        ILogger<FakeApplication> logger = Services.GetInitLogger<FakeApplication>();

        Modules = LoadModules(services);

        logger.LogDebug("模块加载顺序：{Links}", Modules.Select(x => x.Type.Name).JoinAsString(" -> "));

        ConfigureServices();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="FakeInitializationException"></exception>
    public void ConfigureServices()
    {
        if (_configuredServices)
        {
            throw new FakeInitializationException($"{nameof(ConfigureServices)}已经调用过了，不要重复调用");
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
                ExceptionDispatchInfo.Capture(new FakeInitializationException(
                    $"{module.Type.FullName}在{nameof(IConfigureServicesLifecycle.PreConfigureServices)} 阶段发生异常。",
                    ex)).Throw();
            }
        }

        // ConfigureServices
        var assemblies = new HashSet<Assembly>();
        foreach (var module in Modules)
        {
            if (module.Instance is FakeModule { SkipServiceRegistration: false })
            {
                var assembly = module.Type.Assembly;
                if (!assemblies.Contains(assembly))
                {
                    Services.RegisterAssembly(assembly);
                    assemblies.Add(assembly);
                }
            }

            try
            {
                module.Instance.ConfigureServices(context);
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(new FakeInitializationException(
                    $"{module.Type.FullName}在{nameof(IConfigureServicesLifecycle.ConfigureServices)}阶段发生异常。",
                    ex)).Throw();
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
                ExceptionDispatchInfo.Capture(new FakeInitializationException(
                    $"{module.Type.FullName}在{nameof(IConfigureServicesLifecycle.PostConfigureServices)}阶段发生异常。",
                    ex)).Throw();
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
                ExceptionDispatchInfo.Capture(new FakeInitializationException(
                    $"{module.Type.FullName}在{nameof(IShutdownLifecycle.Shutdown)}阶段发生异常。",
                    ex)).Throw();
            }
        }
    }

    public virtual void Dispose()
    {
        Shutdown();
    }

    public void InitializeApplication(IServiceProvider? serviceProvider = null)
    {
        serviceProvider ??= Services.BuildServiceProviderFromFactory().CreateScope().ServiceProvider;

        // tips：正式赋值ServiceProvider
        SetServiceProvider(serviceProvider);

        InitializeModules();
    }

    protected virtual void InitializeModules()
    {
        if (_initializedModules)
        {
            throw new FakeInitializationException($"{nameof(InitializeModules)}已经调用过了，不要重复调用");
        }

        Debug.Assert(_serviceProvider != null);
        ThrowHelper.ThrowIfNull(_serviceProvider);

        WriteInitLogs(_serviceProvider);

        var context = new ApplicationConfigureContext(_serviceProvider);

        // PreConfigure
        foreach (var module in Modules)
        {
            try
            {
                module.Instance.PreConfigureApplication(context);
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(new FakeInitializationException(
                    $"{module.Type.FullName}在{nameof(IConfigureApplicationLifecycle.PreConfigureApplication)}阶段发生异常。",
                    ex)).Throw();
            }
        }

        // Configure
        foreach (var module in Modules)
        {
            try
            {
                module.Instance.ConfigureApplication(context);
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(new FakeInitializationException(
                    $"{module.Type.FullName}在{nameof(IConfigureApplicationLifecycle.ConfigureApplication)}阶段发生异常。",
                    ex)).Throw();
            }
        }

        // PostConfigure
        foreach (var module in Modules)
        {
            try
            {
                module.Instance.PostConfigureApplication(context);
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(new FakeInitializationException(
                    $"{module.Type.FullName}在{nameof(IConfigureApplicationLifecycle.PostConfigureApplication)}阶段发生异常。",
                    ex)).Throw();
            }
        }

        _initializedModules = true;
    }

    protected virtual void SetServiceProvider(IServiceProvider? serviceProvider)
    {
        _serviceProvider = serviceProvider ?? Services.BuildServiceProvider();
        _serviceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = _serviceProvider;
    }

    private IReadOnlyList<IModuleDescriptor> LoadModules(IServiceCollection services)
    {
        return services.GetInstance<IModuleLoader>().LoadModules(services, StartupModuleType);
    }

    private static string GetApplicationName(FakeApplicationCreationOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.ApplicationName))
        {
            return options.ApplicationName;
        }

        var configuration = options.Services.GetConfigurationOrNull();

        if (configuration == default) return Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty;

        return configuration[nameof(IApplicationInfo.ApplicationName)] ?? string.Empty;
    }

    private void WriteInitLogs(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<FakeApplication>>();

        var initLogger = serviceProvider.GetRequiredService<IInitLoggerFactory>().Create<FakeApplication>();

        foreach (var entry in initLogger.Entries)
        {
            logger.Log(entry.LogLevel, entry.EventId, entry.State, entry.Exception, entry.Formatter);
        }

        initLogger.Entries.Clear();
    }


    private void AddFakeCoreServices(IServiceCollection services, FakeApplicationCreationOptions creationOptions)
    {
        var configuration = services.GetInstanceOrNull<IConfiguration>();
        // 添加配置
        if (configuration == null)
        {
            configuration = FakeConfigurationHelper.BuildConfiguration(creationOptions.Configuration);
            services.ReplaceConfiguration(configuration);
        }

        _configuration = configuration;

        // 添加日志
        if (services.GetInstanceOrNull<ILoggerFactory>() == null)
        {
            services.AddLogging(logging =>
            {
                logging.AddConfiguration(configuration!.GetSection("Logging"));
                logging.AddConsole();
            });
        }

        var assemblyScanner = new FakeAssemblyScanner(this);
        var typeScanner = new FakeAssemblyTypeScanner(assemblyScanner);

        services.TryAddSingleton<IModuleLoader>(new FakeModuleLoader());
        services.TryAddSingleton<IAssemblyScanner>(assemblyScanner);
        services.TryAddSingleton<ITypeScanner>(typeScanner);

        services.TryAddSingleton<IInitLoggerFactory>(new DefaultInitLoggerFactory());
    }
}