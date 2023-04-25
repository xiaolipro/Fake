using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Aggregates.BuyerAggregate;

namespace AppTests.EntityConfigurations
{
    class PaymentMethodEntityTypeConfiguration
        : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> paymentConfiguration)
        {
            paymentConfiguration.ToTable("paymentmethods", OrderingContext.DefaultSchema);

            paymentConfiguration.HasKey(b => b.Id);

            paymentConfiguration.Ignore(b => b.DomainEvents);

            paymentConfiguration.Property<int>("BuyerId")
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
                .Property<int>("_cardTypeId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CardTypeId")
                .IsRequired();

            paymentConfiguration.HasOne(p => p.CardType)
                .WithMany()
                .HasForeignKey("_cardTypeId");
        }
    }
}
