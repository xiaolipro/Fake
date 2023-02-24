using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Fake.Modularity;

internal static class FakeModuleHelper
{
    private static void CheckFakeModuleType(Type moduleType)
    {
        if (!FakeModuleApplication.IsFakeModule(moduleType))
        {
            throw new ArgumentException("给定的类型不是Fake模块: " + moduleType.AssemblyQualifiedName);
        }
    }
    
    /// <summary>
    /// 寻找以该模块为起点的所有模块类型（包括该模块本身）
    /// </summary>
    /// <param name="startupModuleType">启动模块</param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static List<Type> FindAllModuleTypes(Type startupModuleType, ILogger logger)
    {
        var moduleTypes = new List<Type>();
        logger.Log(LogLevel.Information, "加载Fake模块:");
        AddModuleAndDependenciesRecursively(moduleTypes, startupModuleType, logger);
        return moduleTypes;
    }
    
    private static void AddModuleAndDependenciesRecursively(
        List<Type> moduleTypes,
        Type moduleType,
        ILogger logger,
        int depth = 0)
    {
        CheckFakeModuleType(moduleType);

        if (moduleTypes.Contains(moduleType))
        {
            return;
        }

        moduleTypes.Add(moduleType);
        logger.Log(LogLevel.Information, $"{new string(' ', depth * 2)}- {moduleType.FullName}");

        foreach (var dependedModuleType in FindDependedModuleTypes(moduleType))
        {
            AddModuleAndDependenciesRecursively(moduleTypes, dependedModuleType, logger, depth + 1);
        }
    }
    
    public static List<Type> FindDependedModuleTypes(Type moduleType)
    {
        CheckFakeModuleType(moduleType);

        var dependencies = new List<Type>();

        var dependencyDescriptors = moduleType
            .GetCustomAttributes()
            .OfType<IDependsOnProvider>();

        foreach (var descriptor in dependencyDescriptors)
        {
            foreach (var dependedModuleType in descriptor.GetDependedTypes())
            {
                dependencies.TryAdd(dependedModuleType);
            }
        }

        return dependencies;
    }
}