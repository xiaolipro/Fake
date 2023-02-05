namespace Fake.Threading;

public interface IScopeProvider<T>
{
    /// <summary>
    /// 获取当前作用域中上下文的值
    /// </summary>
    /// <param name="contextKey"></param>
    /// <returns></returns>
    T GetContext(string contextKey);

    /// <summary>
    /// 开启一个新的作用域
    /// </summary>
    /// <param name="contextKey"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    IDisposable BeginScope(string contextKey, T value);
}