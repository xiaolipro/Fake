using Fake.SqlSugarCore;
using SqlSugar;

public class OrderingContext : SugarDbContext
{
    public OrderingContext(SugarDbConnOptions options) : base(options)
    {
    }

    protected override ConfigureExternalServices ConfigureExternalServices()
    {
        return base.ConfigureExternalServices();
    }
}