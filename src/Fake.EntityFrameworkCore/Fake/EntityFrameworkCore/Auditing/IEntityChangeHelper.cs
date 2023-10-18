using System.Collections.Generic;
using Fake.Auditing;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fake.EntityFrameworkCore.Auditing;

public interface IEntityChangeHelper
{
    [CanBeNull]
    List<EntityChangeInfo> CreateChangeList(IEnumerable<EntityEntry> entityEntries);

    void UpdateChangeList([CanBeNull] List<EntityChangeInfo> entityChanges);
}