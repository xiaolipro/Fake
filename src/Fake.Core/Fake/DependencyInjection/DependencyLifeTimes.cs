namespace Fake.DependencyInjection;

/// <summary>
/// 瞬态生命周期，每次注入都是新的实例
/// </summary>
public interface ITransientDependency
{
}

/// <summary>
/// 作用域生命周期，每个scope中都是单例存在
/// </summary>
public interface IScopedDependency
{
}

/// <summary>
/// 单例生命周期，整个生命周期都共享同一个实例
/// </summary>
public interface ISingletonDependency
{
}