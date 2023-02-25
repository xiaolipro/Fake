using System.Collections.Concurrent;

namespace System.Collections.Generic;

public static class FakeConcurrentDictionaryExtensions
{
    /// <summary>
    /// 根据key从字典中寻找值
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <returns>key如果不存在，则返回默认值</returns>
    public static TValue GetOrDefault<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
    {
        return dictionary.TryGetValue(key, out var obj) ? obj : default;
    }
}