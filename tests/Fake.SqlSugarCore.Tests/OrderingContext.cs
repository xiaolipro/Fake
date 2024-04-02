using Fake.SqlSugarCore;
using SqlSugar;

public class OrderingContext : SugarDbContext<OrderingContext>
{
    public OrderingContext(SugarDbConnOptions<OrderingContext> options) : base(options)
    {
        System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + base.GetHashCode());
    }

    protected override ConfigureExternalServices ConfigureExternalServices()
    {
        return base.ConfigureExternalServices();
    }
}