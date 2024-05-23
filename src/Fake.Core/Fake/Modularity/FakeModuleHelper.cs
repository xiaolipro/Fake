using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Fake.Modularity;

public static class FakeModuleHelper
{
    public static List<Type> AllModuleTypes = [];

    /// <summary>
    /// 确保依赖了给定模块，如果没有则抛出异常
    /// </summary>
    /// <typeparam name="TModule">给定模块</typeparam>
    /// <exception cref="FakeException">没有依赖给定模块</exception>
    public static void EnsureDependsOn<TModule>() where TModule : FakeModule
    {
        if (AllModuleTypes.Contains(typeof(TModule))) return;

        throw new FakeException($"请检查是否依赖模块：{typeof(TModule).FullName}");
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
        logger.Log(LogLevel.Debug, "开始寻找Fake模块:");
        AddModuleAndDependenciesRecursively(moduleTypes, startupModuleType, logger);
        return AllModuleTypes = moduleTypes;
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
        logger.Log(LogLevel.Debug, $"{new string(' ', depth * 2)}- {moduleType.FullName}");

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

    private static void CheckFakeModuleType(Type moduleType)
    {
        var typeInfo = moduleType.GetTypeInfo();

        if (!typeInfo.IsClass &&
            !typeInfo.IsAbstract &&
            !typeInfo.IsGenericType &&
            typeof(IFakeModule).GetTypeInfo().IsAssignableFrom(moduleType))
        {
            throw new ArgumentException("给定的类型不是Fake模块: " + moduleType.AssemblyQualifiedName);
        }
    }
}