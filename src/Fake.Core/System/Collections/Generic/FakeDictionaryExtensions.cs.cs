namespace System.Collections.Generic;

public static class FakeDictionaryExtensions
{
    /// <summary>
    /// 从字段中获取给定key的值，若不存在，则返回默认值
    /// </summary>
    /// <param name="dictionary">Dictionary to check and get</param>
    /// <param name="key">Key to find the value</param>
    /// <typeparam name="TKey">Type of the key</typeparam>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <returns>Value if found, default if can not found.</returns>
    public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        return dictionary.TryGetValue(key, out var value) ? value : default;
    }
    
    /// <summary>
    /// 从字段中获取给定key的值，若不存在，则根据工厂新增，并返回工厂值
    /// </summary>
    /// <param name="dictionary">Dictionary to check and get</param>
    /// <param name="key">Key to find the value</param>
    /// <param name="valueFactory">如果在字典中找不到key，则用于值用该工厂创建</param>
    /// <typeparam name="TKey">Type of the key</typeparam>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <returns>Value if found, default if can not found.</returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
    {
        return dictionary.GetOrAdd(key, k => valueFactory());
    }
    
    /// <summary>
    /// Gets a value from the dictionary with given key. Returns default value if can not find.
    /// </summary>
    /// <param name="dictionary">Dictionary to check and get</param>
    /// <param name="key">Key to find the value</param>
    /// <param name="factory">A factory method used to create the value if not found in the dictionary</param>
    /// <typeparam name="TKey">Type of the key</typeparam>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <returns>Value if found, default if can not found.</returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
    {
        if (dictionary.TryGetValue(key, out var obj))
        {
            return obj;
        }

        return dictionary[key] = factory(key);
    }
}