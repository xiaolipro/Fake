using Fake;

namespace System.Collections.Generic;

public static class FakeDictionaryExtensions
{
    /// <summary>
    /// 从字段中获取给定key的值，若不存在，则返回默认值
    /// </summary>
    /// <param name="dictionary">给定字典</param>
    /// <param name="key">给定key</param>
    /// <returns>存在直接返回，不存在则返回默认值</returns>
    public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        return dictionary.TryGetValue(key, out var value) ? value : default;
    }

    /// <summary>
    /// 从字段中获取给定key的值，若不存在，则插入
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns>存在直接返回，不存在则新增并返回factory value</returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        TValue value)
    {
        return dictionary.GetOrAdd(key, _ => value);
    }

    /// <summary>
    /// 从字段中获取给定key的值，若不存在，则根据工厂新增，并返回工厂值
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="valueFactory">如果在字典中找不到值，则用于创建该值的工厂方法</param>
    /// <returns>存在直接返回，不存在则新增并返回factory value</returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        Func<TValue> valueFactory)
    {
        return dictionary.GetOrAdd(key, _ => valueFactory());
    }

    /// <summary>
    /// 根据给定key获取值，key不存在则新增，值由factory生成
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key">给定key</param>
    /// <param name="valueFactory">如果在字典中找不到值，则用于创建该值的工厂方法</param>
    /// <returns>存在直接返回，不存在则新增并返回factory value</returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        Func<TKey, TValue> valueFactory)
    {
        ThrowHelper.ThrowIfNull(key);
        ThrowHelper.ThrowIfNull(valueFactory);

        if (dictionary.TryGetValue(key, out var obj))
        {
            return obj;
        }

        return dictionary[key] = valueFactory(key);
    }

    /// <summary>
    /// 尝试添加项到集合，项不存在则成功添加并返回true，否则直接返回false
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
    public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        ThrowHelper.ThrowIfNull(key, nameof(key));
        if (dictionary.ContainsKey(key))
        {
            return false;
        }

        dictionary[key] = value;
        return true;
    }
}