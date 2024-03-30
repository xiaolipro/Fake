using Fake.SqlSugarCore;

public class OrderingContext : SugarDbContext
{
    public OrderingContext(SugarDbConnOptions options) : base(options)
    {
    }
}