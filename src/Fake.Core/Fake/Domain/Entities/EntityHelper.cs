using System;
using Fake.Reflection;

namespace Fake.Domain.Entities;

public static class EntityHelper
{
    public static bool EntityEquals(IEntity a, IEntity b)
    {
        if (a == null || b == null) return false;

        // 引用相等一定相等
        if (ReferenceEquals(a, b)) return true;

        if (a.GetType() != b.GetType()) return false;

        if (IsDefaultKeys(a) || IsDefaultKeys(b)) return false;

        object[] keysA = a.GetKeys(), keysB = b.GetKeys();
        for (int i = 0; i < keysA.Length; i++)
        {
            if (keysA[i] != keysB[i]) return false;
        }

        return true;
    }

    public static bool IsDefaultKey(object value)
    {
        if (value == null) return true;

        var type = value.GetType();

        //Workaround for EF Core since it sets int/long to min value when attaching to DbContext
        if (type == typeof(int))
        {
            return Convert.ToInt32(value) <= 0;
        }

        if (type == typeof(long))
        {
            return Convert.ToInt64(value) <= 0;
        }

        return value.Equals(Activator.CreateInstance(type));
    }

    public static bool IsDefaultKeys(IEntity entity)
    {
        foreach (var key in entity.GetKeys())
        {
            if (!IsDefaultKey(key))
            {
                return false;
            }
        }

        return true;
    }


    /// <summary>
    /// 为实体id赋值
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="idFactory"></param>
    /// <param name="checkIgnore"></param>
    /// <typeparam name="TKey"></typeparam>
    public static void TrySetId<TKey>(IEntity<TKey> entity, Func<TKey> idFactory, bool checkIgnore = false)
    {
        var ignoreAttributeTypes = checkIgnore ? new[] { typeof(DisableIdGenerationAttribute) } : default;
        ReflectionHelper.TrySetProperty(entity, x => x.Id, idFactory, ignoreAttributeTypes);
    }
}