using System.Diagnostics;
using System.Reflection;

namespace Fake.Domain;

/// <summary>
/// 枚举基类
/// </summary>
public abstract class Enumeration : IComparable
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Enumeration enumeration) return false;

        if (GetType() != obj.GetType()) return false;

        return Id == enumeration.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    public int CompareTo(object obj)
    {
        var enumeration = obj as Enumeration;
        Debug.Assert(enumeration != null);
        return Id.CompareTo(enumeration.Id);
    }

    #region Utils

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(x => x.FieldType == type)
            .Select(x => x.GetValue(default) as T);

        return fields;
    }

    public static T FromValue<T>(int value) where T : Enumeration
    {
        return Parse<T, int>(value, "value", x => x.Id == value);
    }

    public static T FromDisplayName<T>(string displayName) where T : Enumeration
    {
        return Parse<T, string>(displayName, "display name", x => x.Name == displayName);
    }


    private static T Parse<T, TV>(TV value, string description, Func<T, bool> predicate) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);
        if (matchingItem is null)
        {
            throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
        }

        return matchingItem;
    }

    #endregion
}