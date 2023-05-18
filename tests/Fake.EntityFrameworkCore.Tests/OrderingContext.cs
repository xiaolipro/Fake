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

    /// <summary>
    /// 是否只读
    /// </summary>
    private bool IsReadOnly { get; }

    public OrderingContext(DbContextOptions<OrderingContext> options, IServiceProvider serviceProvider, bool isReadOnly) : base(options, serviceProvider)
    {
        IsReadOnly = isReadOnly;
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

    public override int SaveChanges() => IsReadOnly ? base.SaveChanges() : throw new InvalidOperationException("此DbContext只读");

    public override int SaveChanges(bool acceptAllChangesOnSuccess) => IsReadOnly ? base.SaveChanges(acceptAllChangesOnSuccess) : throw new InvalidOperationException("此DbContext只读");

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new()) => IsReadOnly ? base.SaveChangesAsync(cancellationToken) : throw new InvalidOperationException("此DbContext只读");

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new()) => IsReadOnly ? base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken) : throw new InvalidOperationException("此DbContext只读");
}