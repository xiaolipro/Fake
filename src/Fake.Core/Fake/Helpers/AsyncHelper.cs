using Nito.AsyncEx;

namespace Fake.Helpers;

public static class AsyncHelper
{
    /// <summary>
    /// 同步运行任务
    /// </summary>
    /// <param name="func"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static TResult RunSync<TResult>(Func<Task<TResult>> func)
    {
        return AsyncContext.Run(func);
    }

    /// <summary>
    /// 同步运行任务
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public static void RunSync(Func<Task> func)
    {
        AsyncContext.Run(func);
    }

    public static void RunSync(Task task)
    {
        AsyncContext.Run(() => task);
    }
}