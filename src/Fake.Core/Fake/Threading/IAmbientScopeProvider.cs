namespace Fake.Threading;

public interface IAmbientScopeProvider<T>
{
    /// <summary>
    /// 获取当前所在作用域
    /// </summary>
    /// <param name="contextKey">上下文key</param>
    /// <returns></returns>
    T GetValue(string contextKey);

    /// <summary>
    /// 开启一个新的作用域
    /// </summary>
    /// <param name="contextKey"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    IDisposable BeginScope(string contextKey, T value);
}