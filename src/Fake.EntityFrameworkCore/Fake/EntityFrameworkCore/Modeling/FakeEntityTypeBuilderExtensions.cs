using System;
using Fake.Domain.Entities.Auditing;
using Fake.EntityFrameworkCore.ValueComparers;
using Fake.EntityFrameworkCore.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fake.EntityFrameworkCore.Modeling;

public static class FakeEntityTypeBuilderExtensions
{
    public static EntityTypeBuilder TryConfigureVersionNum(this EntityTypeBuilder builder)
    {
        if (builder.Metadata.ClrType.IsAssignableTo<IHasVersionNum>())
        {
            builder.Property(nameof(IHasVersionNum.VersionNum))
                .HasColumnName(nameof(IHasVersionNum.VersionNum))
                // 当SaveChanges时，它会自动检查并发标记的值是否与原值匹配，如果不匹配就会抛出DbUpdateConcurrencyException异常
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }

        return builder;
    }

    public static EntityTypeBuilder TryConfigureExtraProperties(this EntityTypeBuilder builder)
    {
        if (!builder.Metadata.ClrType.IsAssignableTo<IHasExtraProperties>())
        {
            return builder;
        }

        builder.Property(nameof(IHasExtraProperties.ExtraProperties))
            .HasColumnName(nameof(IHasExtraProperties.ExtraProperties))
            .HasConversion(new FakeJsonValueConverter<ExtraPropertyDictionary>())
            .Metadata.SetValueComparer(new ExtraPropertyDictionaryValueComparer());

        return builder;
    }

    public static EntityTypeBuilder TryConfigureCreator<TUserId>(this EntityTypeBuilder builder)
    {
        if (builder.Metadata.ClrType.IsAssignableTo(typeof(IHasCreator<>)))
        {
            builder.Property(nameof(IHasCreator<TUserId>.CreatorId))
                .HasColumnName(nameof(IHasCreator<TUserId>.CreatorId))
                .IsRequired();
            
            builder.Property(nameof(IHasCreationTime.CreationTime))
                .HasColumnName(nameof(IHasCreationTime.CreationTime))
                .IsRequired();
        }

        return builder;
    }

    public static EntityTypeBuilder TryConfigureModifier<TUserId>(this EntityTypeBuilder builder)
    {
        if (builder.Metadata.ClrType.IsAssignableTo<IHasModifier<TUserId>>())
        {
            builder.Property(nameof(IHasModifier<TUserId>.LastModifierId))
                .HasColumnName(nameof(IHasModifier<TUserId>.LastModifierId))
                .IsRequired();

            builder.Property(nameof(IHasModificationTime.LastModificationTime))
                .HasColumnName(nameof(IHasModificationTime.LastModificationTime))
                .IsRequired();
        }

        return builder;
    }

    public static EntityTypeBuilder TryConfigureSoftDelete(this EntityTypeBuilder builder)
    {
        if (builder.Metadata.ClrType.IsAssignableTo<ISoftDelete>())
        {
            builder.Property(nameof(ISoftDelete.IsDeleted))
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName(nameof(ISoftDelete.IsDeleted));
            
            builder.Ignore(nameof(ISoftDelete.HardDeleted));
        }
        return builder;
    }
}