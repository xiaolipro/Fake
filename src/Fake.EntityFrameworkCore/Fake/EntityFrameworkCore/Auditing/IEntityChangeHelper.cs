using System.Collections.Generic;
using Fake.Auditing;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fake.EntityFrameworkCore.Auditing;

public interface IEntityChangeHelper
{
    List<EntityChangeInfo> CreateChangeList(IEnumerable<EntityEntry> entityEntries);

    void UpdateChangeList(List<EntityChangeInfo> entityChanges);
}