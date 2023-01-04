namespace Bang.Modularity;

public interface IModuleLoader
{
    [NotNull]
    IBangModuleDescriptor[] LoadModules(
        [NotNull] IServiceCollection services,
        [NotNull] Type startupModuleType
    );
}

public class ModuleLoader : IModuleLoader
{
    public IBangModuleDescriptor[] LoadModules(IServiceCollection services, Type startupModuleType)
    {
        Check.NotNull(services, nameof(services));
        Check.NotNull(startupModuleType, nameof(startupModuleType));
        
        var descriptors = GetModuleDescriptors(services, startupModuleType);

        descriptors = descriptors.SortByDependencies(m => m.Dependencies);
        return descriptors.ToArray();
    }
    private List<IBangModuleDescriptor> GetModuleDescriptors(IServiceCollection services, Type startupModuleType)
    {
        var descriptors = new List<IBangModuleDescriptor>();
        FillModuleDescriptors(descriptors, services, startupModuleType);
        SetModuleDependencies(descriptors);

        return descriptors;
    }

    protected virtual void SetModuleDependencies(List<IBangModuleDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            foreach (var dependedModuleType in BangModuleHelper.FindDependedModuleTypes(descriptor.Type))
            {
                var dependedModule = descriptors.FirstOrDefault(m => m.Type == dependedModuleType);
                if (dependedModule == null)
                {
                    throw new BangException($"无法找到 {descriptor.Type.AssemblyQualifiedName} 所依赖的模块 {dependedModuleType.AssemblyQualifiedName}");
                }

                descriptor.AddDependency(dependedModule);
            }
        }
    }

    protected virtual void FillModuleDescriptors(List<IBangModuleDescriptor> descriptors, IServiceCollection services, Type startupModuleType)
    {
        var logger = services.GetLogger<IModuleLoader>();

        foreach (var moduleType in BangModuleHelper.FindAllModuleTypes(startupModuleType,logger))
        {
            var descriptor = CreateModuleDescriptor(services, moduleType);
            descriptors.Add(descriptor);
        }
    }
    
    protected virtual IBangModuleDescriptor CreateModuleDescriptor(IServiceCollection services, Type moduleType)
    {
        var instance = CreateAndRegisterModule(services, moduleType);
        return new BangModuleDescriptor(moduleType, instance);
    }
    
    protected virtual IBangModule CreateAndRegisterModule(IServiceCollection services, Type moduleType)
    {
        var module = Activator.CreateInstance(moduleType) as IBangModule;
        services.AddSingleton(moduleType, module!);
        return module;
    }
}