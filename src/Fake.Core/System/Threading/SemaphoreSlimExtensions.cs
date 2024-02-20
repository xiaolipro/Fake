using Fake;

namespace System.Threading;

public static class SemaphoreSlimExtensions
{
    /// <summary>
    /// 开启一个作用域，创建时调用Wait，销毁时调用Release
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <param name="millisecondsTimeout">默认无期限等待</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    public static IDisposable BeginScope(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout = -1,
        CancellationToken cancellationToken = default)
    {
        var successfully = semaphoreSlim.Wait(millisecondsTimeout, cancellationToken); // -1

        if (successfully)
        {
            return ReleaseInDispose(semaphoreSlim); // +1
        }

        return new DisposableWrapper(() => { });
    }


    private static IDisposable ReleaseInDispose(SemaphoreSlim semaphoreSlim)
    {
        return new DisposableWrapper(() => { semaphoreSlim.Release(); });
    }
}