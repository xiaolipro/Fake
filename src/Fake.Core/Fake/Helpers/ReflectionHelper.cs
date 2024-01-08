using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Fake.Helpers;

public static class ReflectionHelper
{
    private static readonly ConcurrentDictionary<string, PropertyInfo?> PropertiesCaches = new();
    private static readonly ConcurrentDictionary<Assembly, IReadOnlyList<Type>> AssemblyCaches = new();

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
        if (obj is null) return;

        var cacheKey = $"{obj.GetType().FullName}-{propertySelector}-{ignoreAttributeTypes.JoinAsString("-")}";

        var propertyInfo = PropertiesCaches.GetOrAdd(cacheKey, _ =>
        {
            // 必须从字段或属性上读取
            if (propertySelector.Body.NodeType != ExpressionType.MemberAccess) return null;

            var memberExpression = propertySelector.Body.To<MemberExpression>();

            var propertyInfo = obj.GetType().GetProperties().FirstOrDefault(x =>
                x.Name == memberExpression.Member.Name &&
                x.GetSetMethod(true) != null);

            if (propertyInfo == null) return null;

            // 如果定义了忽略特性
            if (ignoreAttributeTypes.Any(t => propertyInfo.IsDefined(t, true))) return null;

            return propertyInfo;
        });

        propertyInfo?.SetValue(obj, valueFactory());
    }

    /// <summary>
    /// 获取给定成员指定特性，如果没有则返回defaultValue
    /// </summary>
    /// <param name="memberInfo"></param>
    /// <param name="defaultValue"></param>
    /// <param name="inherit"></param>
    /// <param name="includeDeclaringType">从定义类型上寻找</param>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    public static TAttribute? GetAttributeOrDefault<TAttribute>(MemberInfo memberInfo,
        TAttribute? defaultValue = default, bool inherit = true, bool includeDeclaringType = false)
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
    public static TInstance CreateInstance<TInstance>(Type type) where TInstance : class
    {
        //TODO: 可以优化
        return Activator.CreateInstance(type).To<TInstance>();
    }

    /// <summary>
    /// 获取成员类型
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Type GetMemberType(this MemberInfo member)
    {
        if (member is PropertyInfo propertyInfo)
            return propertyInfo.PropertyType;
        if (member is MethodInfo methodInfo)
            return methodInfo.ReturnType;
        if (member is FieldInfo fieldInfo)
            return fieldInfo.FieldType;
        if ((object)member == null)
            throw new ArgumentNullException(nameof(member));
        throw new ArgumentOutOfRangeException(nameof(member));
    }

    public static IReadOnlyList<Type> GetAssemblyAllTypes(Assembly assembly)
    {
        try
        {
            return AssemblyCaches.GetOrAdd(assembly, _ => assembly.GetTypes());
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types;
        }
    }
}