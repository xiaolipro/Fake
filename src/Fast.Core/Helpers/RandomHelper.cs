namespace Fast.Core.Helpers;

public static class RandomHelper
{
    private static readonly ThreadLocal<Random> _localRandom  =
        new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));

    /// <summary>
    /// 下一个随机数
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns>返回一个int类型随机数</returns>
    public static int Next(int minValue = 0, int maxValue = Int32.MaxValue)
    {
        return _localRandom.Value.Next(minValue, maxValue);
    }
    
    /// <summary>
    /// 将集合重新洗牌
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    public static List<T> Shuffle<T>(IEnumerable<T> items)
    {
        var original = new List<T>(items);
        var res = new List<T>();

        while (original.Any())
        {
            var idx = Next(0, original.Count);
            res.Add(original[idx]);
            original.RemoveAt(idx);
        }

        return res;
    }

    public static T[] RandomSlice<T>(params T[] array)
    {
        return Shuffle(array).GetRange(0, Next(maxValue:array.Length)).ToArray();
    }
}