using Domain.Aggregates.BuyerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fake.EntityFrameworkCore.Tests.EntityConfigurations;

class PaymentMethodEntityTypeConfiguration
    : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> paymentConfiguration)
    {
        paymentConfiguration.ToTable("paymentmethods", OrderingContext.DefaultSchema);

        paymentConfiguration.HasKey(b => b.Id);

        paymentConfiguration.Property<Guid>("BuyerId")
            .IsRequired();

        paymentConfiguration
            .Property<string>("_cardHolderName")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("CardHolderName")
            .HasMaxLength(200)
            .IsRequired();

        paymentConfiguration
            .Property<string>("_alias")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Alias")
            .HasMaxLength(200)
            .IsRequired();

        paymentConfiguration
            .Property<string>("_cardNumber")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("CardNumber")
            .HasMaxLength(25)
            .IsRequired();

        paymentConfiguration
            .Property<DateTime>("_expiration")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Expiration")
            .HasMaxLength(25)
            .IsRequired();

        paymentConfiguration
            .Property(x => x.CardType)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();
    }
}