namespace Bang.DependencyInjection;

public class ServiceRegisterList : List<IServiceRegistrar>
{
    public ServiceRegisterList()
    {
        Add(new DefaultServiceRegistrar());
    }
}