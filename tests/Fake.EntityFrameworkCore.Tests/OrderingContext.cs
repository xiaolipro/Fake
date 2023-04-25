using AppTests.EntityConfigurations;
using Domain.Aggregates.BuyerAggregate;
using Domain.Aggregates.OrderAggregate;
using Fake.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class OrderingContext: FakeDbContext<OrderingContext>
{
    public const string DefaultSchema = "ordering";
    public DbSet<Order> Orders { get; set; }
    public DbSet<Buyer> Buyers { get; set; }

    public OrderingContext(DbContextOptions<OrderingContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PaymentMethodEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CardTypeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderStatusEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BuyerEntityTypeConfiguration());
    }
}