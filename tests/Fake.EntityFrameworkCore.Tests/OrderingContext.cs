using Domain.Aggregates.BuyerAggregate;
using Domain.Aggregates.OrderAggregate;
using Fake.EntityFrameworkCore.Tests.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Fake.EntityFrameworkCore.Tests;

public class OrderingContext : EfCoreDbContext<OrderingContext>
{
    public const string DefaultSchema = "ordering";
    public DbSet<Order> Orders { get; set; }
    public DbSet<Buyer> Buyers { get; set; }

    public OrderingContext(DbContextOptions<OrderingContext> options) : base(options)
    {
        System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + base.GetHashCode());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PaymentMethodEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BuyerEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}