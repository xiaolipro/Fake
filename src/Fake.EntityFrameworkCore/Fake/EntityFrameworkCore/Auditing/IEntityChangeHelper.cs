using Fake.Auditing;

namespace Fake.EntityFrameworkCore.Auditing;

public interface IEntityChangeHelper
{
    List<EntityChangeInfo>? CreateChangeList(IEnumerable<EntityEntry> entityEntries);

    void UpdateChangeList(List<EntityChangeInfo> entityChanges);
}