namespace Fake.Proxy;

public static class ProxyHelper
{
    private const string ProxyNamespace = "Castle.Proxies";

    public static Type GetUnProxiedType(object obj)
    {
        var type = obj.GetType();
        if (type.Namespace != ProxyNamespace) return type;
    }
}