namespace System.Collections.Generic;

public static class FakeListExtensions
{
    public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> action)
    {
        foreach (var item in list)
        {
            await action(item);
        }
    }
}