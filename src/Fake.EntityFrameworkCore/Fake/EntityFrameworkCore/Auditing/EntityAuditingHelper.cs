using System;
using System.Collections.Generic;
using System.Linq;
using Fake.Auditing;
using Fake.Data;
using Fake.Domain.Entities;
using Fake.Domain.Entities.Auditing;
using Fake.Json;
using Fake.Timing;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;

namespace Fake.EntityFrameworkCore.Auditing;

public class EntityAuditingHelper : IEntityChangeHelper
{
    protected IAuditingStore AuditingStore { get; }
    protected IFakeJsonSerializer JsonSerializer { get; }
    protected FakeAuditingOptions Options { get; }
    protected IAuditingHelper AuditingHelper { get; }
    protected IFakeClock Clock { get; }

    public EntityAuditingHelper(
        IAuditingStore auditingStore,
        IOptions<FakeAuditingOptions> options,
        IFakeClock clock,
        IFakeJsonSerializer jsonSerializer,
        IAuditingHelper auditingHelper)
    {
        Clock = clock;
        AuditingStore = auditingStore;
        JsonSerializer = jsonSerializer;
        AuditingHelper = auditingHelper;
        Options = options.Value;
    }

    #region 创建实体变更集合

    public List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries)
    {
        var changes = new List<EntityChangeInfo>();

        foreach (var entry in entityEntries)
        {
            if (!ShouldSaveEntityChange(entry)) continue;

            var change = CreateEntityChangeOrNull(entry);

            if (change != null) changes.Add(change);
        }

        return changes;
    }

    [CanBeNull]
    protected virtual EntityChangeInfo CreateEntityChangeOrNull(EntityEntry entry)
    {
        var entity = entry.Entity;

        EntityChangeType changeType;
        switch (entry.State)
        {
            case EntityState.Added:
                changeType = EntityChangeType.Created;
                break;
            case EntityState.Deleted:
                changeType = EntityChangeType.Deleted;
                break;
            case EntityState.Modified:
                changeType = IsSoftDeleted(entry) ? EntityChangeType.Deleted : EntityChangeType.Updated;
                break;
            case EntityState.Detached:
            case EntityState.Unchanged:
            default:
                return null;
        }

        var entityId = GetEntityId(entity);
        if (entityId == null && changeType != EntityChangeType.Created)
        {
            return null;
        }

        var entityChange = new EntityChangeInfo
        {
            ChangeType = changeType,
            EntityEntry = entry,
            EntityId = entityId,
            EntityTypeFullName = entity.GetType().FullName,
            PropertyChanges = GetPropertyChanges(entry),
        };

        return entityChange;
    }

    protected virtual List<EntityPropertyChangeInfo> GetPropertyChanges(EntityEntry entityEntry)
    {
        var propertyChanges = new List<EntityPropertyChangeInfo>();
        var properties = entityEntry.Metadata.GetProperties();
        var isCreated = entityEntry.State == EntityState.Added;
        var isDeleted = IsDeleted(entityEntry);

        foreach (var property in properties)
        {
            var propertyEntry = entityEntry.Property(property.Name);

            if (IsSoftDeleted(entityEntry)) continue;
            if (ShouldSaveEntityPropertyChange(propertyEntry))
            {
                propertyChanges.Add(new EntityPropertyChangeInfo
                {
                    NewValue = isDeleted
                        ? null
                        : JsonSerializer.Serialize(propertyEntry.CurrentValue)
                            .TruncateWithSuffix(Options.EntityChangeOptions.ValueMaxLength),
                    OriginalValue = isCreated
                        ? null
                        : JsonSerializer.Serialize(propertyEntry.OriginalValue)
                            .TruncateWithSuffix(Options.EntityChangeOptions.ValueMaxLength),
                    PropertyName = property.Name,
                    PropertyTypeFullName = property.ClrType.FullName
                });
            }
        }

        return propertyChanges;
    }

    protected virtual bool IsDeleted(EntityEntry entityEntry)
    {
        return entityEntry.State == EntityState.Deleted || IsSoftDeleted(entityEntry);
    }

    protected virtual bool IsSoftDeleted(EntityEntry entityEntry)
    {
        var entity = entityEntry.Entity;
        return entity is ISoftDelete && entity.Cast<ISoftDelete>().IsDeleted;
    }

    [CanBeNull]
    protected virtual string GetEntityId(object entityAsObj)
    {
        if (entityAsObj is not IEntity entity)
        {
            throw new FakeException(
                $"实体必须实现{typeof(IEntity).AssemblyQualifiedName}接口！给定的实体并没有实现它: {entityAsObj.GetType().AssemblyQualifiedName}");
        }

        var keys = entity.GetKeys();
        return keys.All(k => k == null) ? null : keys.JoinAsString(",");
    }

    protected virtual bool ShouldSaveEntityChange(EntityEntry entry)
    {
        if (!Options.EntityChangeOptions.IsEnabled) return false;
        if (entry.State.IsIn(EntityState.Detached, EntityState.Unchanged)) return false;

        var entityType = entry.Metadata.ClrType;

        if (!EntityHelper.IsEntity(entityType)) return false;

        if (AuditingHelper.IsAuditEntity(entityType)) return true;

        return false;
    }

    protected virtual bool ShouldSaveEntityPropertyChange(PropertyEntry propertyEntry)
    {
        if (propertyEntry.Metadata.Name.IsIn(Options.EntityChangeOptions.IgnoreProperties)) return false;

        var propertyInfo = propertyEntry.Metadata.PropertyInfo;
        if (propertyInfo == null) return false;
        if (propertyInfo.IsDefined(typeof(DisableAuditingAttribute), true)) return false;

        if (propertyEntry.OriginalValue is ExtraPropertyDictionary originalValue &&
            propertyEntry.CurrentValue is ExtraPropertyDictionary currentValue)
        {
            if (originalValue.IsNullOrEmpty() && currentValue.IsNullOrEmpty()) return false;
            if (!originalValue.Select(x => x.Key).SequenceEqual(currentValue.Select(x => x.Key))) return true;
            if (!originalValue.Select(x => x.Value).SequenceEqual(currentValue.Select(x => x.Value))) return true;
            return false;
        }

        return !(propertyEntry.OriginalValue?.Equals(propertyEntry.CurrentValue) ?? propertyEntry.CurrentValue == null);
    }

    #endregion


    public void UpdateChangeList(List<EntityChangeInfo> entityChanges)
    {
        foreach (var entityChange in entityChanges)
        {
            entityChange.ChangeTime = GetChangeTime(entityChange);

            var entityEntry = entityChange.EntityEntry.Cast<EntityEntry>();
            entityChange.EntityId = GetEntityId(entityEntry.Entity);
        }
    }

    protected virtual DateTime GetChangeTime(EntityChangeInfo entityChange)
    {
        var entity = entityChange.EntityEntry.Cast<EntityEntry>().Entity;
        switch (entityChange.ChangeType)
        {
            case EntityChangeType.Created:
                return (entity as IHasCreationTime)?.CreationTime ?? Clock.Now;
            case EntityChangeType.Updated:
                return (entity as IHasModificationTime)?.LastModificationTime ?? Clock.Now;
            default:
                throw new FakeException($"Unknown {nameof(EntityChangeInfo)}: {entityChange}");
        }
    }
}