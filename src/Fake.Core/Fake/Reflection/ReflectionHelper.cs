using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Fake.Reflection;

public static class ReflectionHelper
{
    private static readonly ConcurrentDictionary<string, PropertyInfo> CachedPropertiesDic =
        new ConcurrentDictionary<string, PropertyInfo>();

    /// <summary>
    /// 尝试为给定对象的属性赋值
    /// </summary>
    /// <param name="obj">给定对象</param>
    /// <param name="propertySelector">属性选择器</param>
    /// <param name="valueFactory">值工厂</param>
    /// <param name="ignoreAttributeTypes">忽略特性，如果属性标记了，则忽略赋值</param>
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
    /// <param name="includeDeclaringType"></param>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    public static TAttribute GetSingleAttributeOrDefault<TAttribute>(MemberInfo memberInfo,
        TAttribute defaultValue = default, bool inherit = true, bool includeDeclaringType = false)
        where TAttribute : Attribute
    {
        //Get attribute on the member
        if (memberInfo.IsDefined(typeof(TAttribute), inherit))
        {
            return memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).FirstOrDefault() as TAttribute;
        }

        if (!includeDeclaringType) return defaultValue;

        return memberInfo.DeclaringType?.GetType().GetCustomAttributes(typeof(TAttribute), inherit)
            .FirstOrDefault() as TAttribute ?? defaultValue;
    }


    /// <summary>
    /// 获取类型实例
    /// </summary>
    /// <param name="type"></param>
    /// <typeparam name="TInstance"></typeparam>
    /// <returns></returns>
    public static TInstance CreateInstance<TInstance>(Type type) where TInstance: class
    {
        //TODO: 可以优化
        return Activator.CreateInstance(type) as TInstance;
    }
    
}