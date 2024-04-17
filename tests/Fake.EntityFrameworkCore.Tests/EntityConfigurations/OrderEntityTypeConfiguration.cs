using Domain.Aggregates.BuyerAggregate;
using Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fake.EntityFrameworkCore.Tests.EntityConfigurations;

class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> orderConfiguration)
    {
        orderConfiguration.ToTable("orders", OrderingContext.DefaultSchema);

        orderConfiguration.HasKey(o => o.Id);

        //Address value object persisted as owned entity type supported since EF Core 2.0
        orderConfiguration
            .OwnsOne(o => o.Address);

        orderConfiguration
            .Property(x => x.BuyerId)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired(false);

        orderConfiguration
            .Property(x => x.OrderDate)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();

        orderConfiguration
            .Property(x => x.OrderStatus)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();

        orderConfiguration
            .Property(x => x.PaymentMethodId)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired(false);

        orderConfiguration
            .Property(x => x.Description)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired(false);

        var navigation = orderConfiguration.Metadata.FindNavigation(nameof(Order.OrderItems));

        // DDD Patterns comment:
        //Set as field (New since EF 1.1) to access the OrderItem collection property through its field
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        orderConfiguration.HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey(x => x.PaymentMethodId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        orderConfiguration.HasOne<Buyer>()
            .WithMany()
            .IsRequired(false)
            .HasForeignKey(x => x.BuyerId);
    }
}