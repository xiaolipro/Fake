using System.Reflection;
using Fake.Modularity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Fake.AspNetCore.Mvc.Conventions;

public class RemoteService2ControllerOptions
{
    internal HashSet<Type> ControllerTypes { get; } = new();
    internal List<Assembly> Assemblies { get; } = new();

    public string RootPath
    {
        get => _rootPath;
        set
        {
            ThrowHelper.ThrowIfNull(value, nameof(value));
            _rootPath = value;
        }
    }

    private string _rootPath = "app";

    public Action<ControllerModel>? ControllerModelConfigureAction { get; set; }

    public void ScanRemoteServices<TModule>() where TModule : IFakeModule
    {
        var assembly = typeof(TModule).Assembly;
        if (Assemblies.Contains(assembly)) return;

        Assemblies.Add(assembly);
        var types = assembly.GetTypes().Where(IsRemoteService);

        foreach (var type in types)
        {
            ControllerTypes.Add(type);
        }
    }

    private static bool IsRemoteService(Type type)
    {
        if (!type.IsPublic || type.IsAbstract || type.IsGenericType)
        {
            return false;
        }

        if (typeof(IRemoteService).IsAssignableFrom(type))
        {
            return true;
        }

        return false;
    }
}