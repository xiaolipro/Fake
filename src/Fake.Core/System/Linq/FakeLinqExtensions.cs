using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq;

public static class FakeLinqExtensions
{
    public static IQueryable<TEntity> OrderBy<TEntity>(
        this IQueryable<TEntity> query,
        Dictionary<string, bool> fields)
        where TEntity : class
    {
        int num = 0;
        foreach (KeyValuePair<string, bool> field in fields)
        {
            query = num != 0
                ? query.ThenBy(field.Key, field.Value)
                : query.OrderBy<TEntity>(field.Key, field.Value);
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
        .FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>)(p =>
            p.Name.Equals(field, StringComparison.OrdinalIgnoreCase)));

    private static LambdaExpression GetOrderExpression(
        Type entityType,
        PropertyInfo propertyInfo)
    {
        ParameterExpression parameterExpression = Expression.Parameter(entityType);
        return Expression.Lambda(Expression.PropertyOrField(parameterExpression, propertyInfo.Name),
            parameterExpression);
    }
}