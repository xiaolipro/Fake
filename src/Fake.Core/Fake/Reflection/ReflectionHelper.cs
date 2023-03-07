using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Fake.Reflection;

public static class ReflectionHelper
{
    private static readonly ConcurrentDictionary<string, PropertyInfo> CachedPropertiesDic =
        new ConcurrentDictionary<string, PropertyInfo>();

    public static void TrySetProperty<TObject, TValue>(TObject obj, Expression<Func<TObject, TValue>> propertySelector,
        Func<TValue> valueFactory, params Type[] ignoreAttributeTypes)
    {
        var cacheKey = $"{obj.GetType().FullName}-{propertySelector}-{ignoreAttributeTypes.JoinAsString("-")}";

        var propertyInfo = CachedPropertiesDic.GetOrAdd(cacheKey, _ =>
        {
            // 如果不是从字段或属性上读取
            if (propertySelector.Body.NodeType != ExpressionType.MemberAccess) return null;

            var propertyInfo = obj.GetType().GetProperties().FirstOrDefault(p =>
            {
                if (p.Name != propertySelector.Body.As<MemberExpression>()!.Member.Name) return false;
                if (p.GetSetMethod(true) == null) return false;
                return true;
            });

            if (propertyInfo == null) return null;

            // 如果定义了忽略特性
            if (ignoreAttributeTypes.Any(t => propertyInfo.IsDefined(t, true))) return null;

            return propertyInfo;
        });
        
        propertyInfo?.SetValue(obj, valueFactory());
    }

    /// <summary>
    /// 获取给定成员指定特性，如果没有则返回<see cref="defaultValue"/>
    /// </summary>
    /// <param name="memberInfo"></param>
    /// <param name="defaultValue"></param>
    /// <param name="inherit"></param>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    public static TAttribute GetSingleAttributeOrDefault<TAttribute>(MemberInfo memberInfo,
        TAttribute defaultValue = default, bool inherit = true)
        where TAttribute : Attribute
    {
        //Get attribute on the member
        if (memberInfo.IsDefined(typeof(TAttribute), inherit))
        {
            return memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().First();
        }

        return defaultValue;
    }
}