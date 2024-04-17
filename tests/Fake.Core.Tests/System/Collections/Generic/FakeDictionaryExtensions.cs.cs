namespace Fake.Core.Tests.System.Collections.Generic;

public static class FakeDictionaryExtensions
{
    /// <summary>
    /// 将字典转化成 <see cref="ExpandoObject"/>
    /// </summary>
    /// <param name="dic">字典的key需是string</param>
    /// <returns></returns>
    public static ExpandoObject ToExpandoObject(this Dictionary<string, object> dic)
    {
        var res = new ExpandoObject();
        var collection = res as ICollection<KeyValuePair<string, object>>;

        foreach (var keyValuePair in dic)
        {
            collection.Add(keyValuePair);
        }

        return res;
    }
}