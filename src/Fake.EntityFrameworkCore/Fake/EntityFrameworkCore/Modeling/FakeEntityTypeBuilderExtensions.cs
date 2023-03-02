using System;
using Fake.Domain.Entities;
using Fake.Domain.Entities.Auditing;
using Fake.EntityFrameworkCore.ValueComparers;
using Fake.EntityFrameworkCore.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fake.EntityFrameworkCore.Modeling;

public static class FakeEntityTypeBuilderExtensions
{
    public static EntityTypeBuilder TryConfigurePrimaryKey<TKey>(this EntityTypeBuilder builder)
    {
        if (builder.Metadata.ClrType.IsAssignableTo<IEntity<TKey>>())
        {
            builder.HasKey(nameof(IEntity<TKey>.Id));
            builder.Property(nameof(IEntity<TKey>.Id))
                .HasColumnName(nameof(IEntity<TKey>.Id));
        }

        return builder;
    }
    
    public static EntityTypeBuilder TryConfigureVersionNum(this EntityTypeBuilder builder)
    {
        if (builder.Metadata.ClrType.IsAssignableTo<IHasVersionNum>())
        {
            builder.Property(nameof(IHasVersionNum.VersionNum))
                .HasColumnName(nameof(IHasVersionNum.VersionNum))
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
        if (builder.Metadata.ClrType.IsAssignableTo<IHasCreator<TUserId>>())
        {
            builder.Property(nameof(IHasCreator<TUserId>.CreatorId))
                .HasColumnName(nameof(IHasCreator<TUserId>.CreatorId))
                .IsRequired();
            
            builder.Property(nameof(IHasCreator<TUserId>.CreationTime))
                .HasColumnName(nameof(IHasCreator<TUserId>.CreationTime))
                .IsRequired();
        }

        return builder;
    }
    
    public static EntityTypeBuilder TryConfigureDeleter<TUserId>(this EntityTypeBuilder builder)
    {
        if (builder.Metadata.ClrType.IsAssignableTo<IHasDeleter<TUserId>>())
        {
            builder.Property(nameof(IHasDeleter<TUserId>.DeleterId))
                .HasColumnName(nameof(IHasDeleter<TUserId>.DeleterId))
                .IsRequired(false);
            
            builder.Property(nameof(IHasDeleter<TUserId>.DeletionTime))
                .HasColumnName(nameof(IHasDeleter<TUserId>.DeletionTime))
                .IsRequired(false);
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
            
            builder.Property(nameof(IHasModifier<TUserId>.LastModificationTime))
                .HasColumnName(nameof(IHasModifier<TUserId>.LastModificationTime))
                .IsRequired();
        }

        return builder;
    }
    
    public static EntityTypeBuilder TryConfigureSoftDelete<TUserId>(this EntityTypeBuilder builder)
    {
        if (builder.Metadata.ClrType.IsAssignableTo<IHasSoftDelete>())
        {
            builder.Property(nameof(IHasSoftDelete.IsDeleted))
                .HasColumnName(nameof(IHasModifier<TUserId>.LastModifierId))
                .IsRequired()
                .HasDefaultValue(false);
            
            builder.Property(nameof(IHasModifier<TUserId>.LastModificationTime))
                .HasColumnName(nameof(IHasModifier<TUserId>.LastModificationTime))
                .IsRequired();
        }

        return builder;
    }
}