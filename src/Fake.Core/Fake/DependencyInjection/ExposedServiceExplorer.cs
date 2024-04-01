namespace Fake.DependencyInjection;

public static class ExposedServiceExplorer
{
    private static readonly ExposeServicesAttribute DefaultExposeServicesAttribute = new()
    {
        ExposeInterface = true,
        ExposeSelf = true
    };

    public static List<ServiceIdentifier> GetExposedServices(Type type)
    {
        return type
            .GetCustomAttributes(true)
            .OfType<IExposedServiceTypesProvider>()
            .DefaultIfEmpty(DefaultExposeServicesAttribute)
            .SelectMany(p => p.GetExposedServices(type))
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