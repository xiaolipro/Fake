namespace Fake.DependencyInjection;

public static class ExposedServiceExplorer
{
    private static readonly ExposeServicesAttribute DefaultExposeServicesAttribute = new();

    public static List<Type> GetExposedServiceTypes(Type type)
    {
        return type
            .GetCustomAttributes(true)
            .OfType<IExposedServiceTypesProvider>()
            .DefaultIfEmpty(DefaultExposeServicesAttribute)
            .SelectMany(p => p.GetExposedServiceTypes(type))
            .Distinct()
            .ToList();
    }

    /// <summary>
    /// 如果你想改变暴露的默认行为，可以在BeforeAddFakeApplication调用此方法
    /// </summary>
    /// <param name="options"></param>
    public static void SetDefaultExposeServicesAttribute(Action<ExposeServicesAttribute> options)
    {
        options.Invoke(DefaultExposeServicesAttribute);
    }
}