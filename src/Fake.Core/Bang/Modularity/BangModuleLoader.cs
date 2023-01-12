using Microsoft.Extensions.Logging;

namespace Fake.Modularity;

public class FakeModuleLoader : IModuleLoader
{
    public IModuleDescriptor[] LoadModules(IServiceCollection services, Type startupModuleType)
    {
        ThrowHelper.ThrowIfNull(services, nameof(services));
        ThrowHelper.ThrowIfNull(startupModuleType, nameof(startupModuleType));
        
        var descriptors = GetModuleDescriptors(services, startupModuleType);

        descriptors = descriptors.SortByDependencies(m => m.Dependencies);
        return descriptors.ToArray();
    }
    private List<IModuleDescriptor> GetModuleDescriptors(IServiceCollection services, Type startupModuleType)
    {
        var descriptors = new List<IModuleDescriptor>();
        FillModuleDescriptors(descriptors, services, startupModuleType);
        SetModuleDependencies(descriptors);

        return descriptors;
    }

    protected virtual void SetModuleDependencies(List<IModuleDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            foreach (var dependedModuleType in FakeModuleHelper.FindDependedModuleTypes(descriptor.Type))
            {
                var dependedModule = descriptors.FirstOrDefault(m => m.Type == dependedModuleType);
                if (dependedModule == null)
                {
                    throw new FakeException($"无法找到{descriptor.Type.AssemblyQualifiedName}所依赖的模块{dependedModuleType.AssemblyQualifiedName}");
                }

                descriptor.AddDependency(dependedModule);
            }
        }
    }

    protected virtual void FillModuleDescriptors(List<IModuleDescriptor> descriptors, IServiceCollection services, Type startupModuleType)
    {
        var logger = services.GetLogger<IModuleLoader>();

        foreach (var moduleType in FakeModuleHelper.FindAllModuleTypes(startupModuleType,logger))
        {
            var descriptor = CreateModuleDescriptor(services, moduleType);
            if (descriptors.Any(x => x.Assembly.Equals(moduleType.Assembly)))
            {
                logger.LogWarning($"程序集{moduleType.AssemblyQualifiedName}内发现多个{nameof(IFakeModule)}");
            }
            descriptors.TryAdd(descriptor);
        }
    }
    
    protected virtual IModuleDescriptor CreateModuleDescriptor(IServiceCollection services, Type moduleType)
    {
        var instance = CreateAndRegisterModule(services, moduleType);
        return new FakeModuleDescriptor(moduleType, instance);
    }
    
    protected virtual IFakeModule CreateAndRegisterModule(IServiceCollection services, Type moduleType)
    {
        var module = Activator.CreateInstance(moduleType) as IFakeModule;
        services.AddSingleton(moduleType, module!);
        return module;
    }
}