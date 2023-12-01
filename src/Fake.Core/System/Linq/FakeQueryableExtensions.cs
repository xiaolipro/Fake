using System.Linq.Expressions;
using System.Reflection;
using Fake;

namespace System.Linq;

public static class FakeQueryableExtensions
{
    /// <summary>
    /// 如果<paramref name="condition"/> is true，按照<paramref name="predicate"/>过滤 <see cref="IQueryable{T}"/>
    /// </summary>
    /// <param name="query">被过滤的query</param>
    /// <param name="condition">过滤条件</param>
    /// <param name="predicate">过滤表达式</param>
    /// <returns>过滤后的结果</returns>
    public static TQueryable WhereIf<T, TQueryable>(this TQueryable query, bool condition,
        Expression<Func<T, bool>>? predicate)
        where TQueryable : IQueryable<T>?
    {
        ThrowHelper.ThrowIfNull(query, nameof(query));

        return condition
            ? (TQueryable)query.Where(predicate)
            : query;
    }

    public static IQueryable<TEntity> OrderBy<TEntity>(
        this IQueryable<TEntity> query,
        Dictionary<string, bool>? fields)
        where TEntity : class
    {
        var num = 0;
        foreach (var field in fields)
        {
            query = num != 0
                ? query.ThenBy(field.Key, field.Value)
                : query.OrderBy(field.Key, field.Value);
            ++num;
        }

        return query;
    }

    public static IQueryable<TEntity> OrderBy<TEntity>(
        this IQueryable<TEntity> query,
        string field,
        bool desc)
        where TEntity : class
    {
        PropertyInfo propertyInfo = GetPropertyInfo(typeof(TEntity), field);
        LambdaExpression orderExpression = GetOrderExpression(typeof(TEntity), propertyInfo);
        return desc
            ? typeof(Queryable).GetMethods()
                .FirstOrDefault(m =>
                    m.Name == "OrderByDescending" && m.GetParameters().Length == 2)
                ?.MakeGenericMethod(typeof(TEntity), propertyInfo.PropertyType).Invoke(null, new object[]
                {
                    query,
                    orderExpression
                }) as IQueryable<TEntity>
            : (IQueryable<TEntity>)typeof(Queryable).GetMethods()
                .FirstOrDefault(m =>
                    m.Name == nameof(OrderBy) && m.GetParameters().Length == 2)
                ?.MakeGenericMethod(typeof(TEntity), propertyInfo.PropertyType).Invoke(null, new object[]
                {
                    query,
                    orderExpression
                });
    }

    private static IQueryable<T> ThenBy<T>(
        this IQueryable<T> query,
        string field,
        bool desc)
        where T : class
    {
        PropertyInfo propertyInfo = GetPropertyInfo(typeof(T), field);
        LambdaExpression orderExpression = GetOrderExpression(typeof(T), propertyInfo);
        return desc
            ? typeof(Queryable).GetMethods()
                .FirstOrDefault(m => m.Name == "ThenByDescending" && m.GetParameters().Length == 2)
                ?.MakeGenericMethod(typeof(T), propertyInfo.PropertyType).Invoke(null, new object[]
                {
                    query,
                    orderExpression
                }) as IQueryable<T>
            : (IQueryable<T>)typeof(Queryable).GetMethods()
                .FirstOrDefault(m => m.Name == nameof(ThenBy) && m.GetParameters().Length == 2)
                ?.MakeGenericMethod(typeof(T), propertyInfo.PropertyType).Invoke(null, new object[]
                {
                    query,
                    orderExpression
                });
    }

    private static PropertyInfo GetPropertyInfo(Type entityType, string field) => entityType.GetProperties()
        .FirstOrDefault(p =>
            p.Name.Equals(field, StringComparison.OrdinalIgnoreCase));

    private static LambdaExpression GetOrderExpression(
        Type entityType,
        PropertyInfo propertyInfo)
    {
        var parameterExpression = Expression.Parameter(entityType);
        return Expression.Lambda(Expression.PropertyOrField(parameterExpression, propertyInfo.Name),
            parameterExpression);
    }
}