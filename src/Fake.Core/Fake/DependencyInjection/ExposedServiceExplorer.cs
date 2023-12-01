namespace Fake.DependencyInjection;

public static class ExposedServiceExplorer
{
    private static readonly ExposeServicesAttribute DefaultExposeServicesAttribute =
        new ExposeServicesAttribute
        {
            ExposeInterface = true,
            ExposeSelf = false
        };

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
}