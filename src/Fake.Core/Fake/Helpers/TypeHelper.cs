namespace Fake.Helpers;

public static class TypeHelper
{
    public static bool IsNullable(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    public static Type GetFirstGenericArgument(Type type)
    {
        if (type.IsGenericType)
        {
            return type.GetGenericArguments().FirstOrDefault();
        }

        return type;
    }
}