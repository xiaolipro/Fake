using System.Collections.Generic;
using Fake.Auditing;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fake.EntityFrameworkCore.Auditing;

public interface IEntityAuditingHelper
{
    List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries);
}
public class EntityAuditingHelper
{
    
}