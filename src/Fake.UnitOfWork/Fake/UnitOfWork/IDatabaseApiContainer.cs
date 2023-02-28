using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;


/// <summary>
/// 数据库api容器
/// </summary>
public interface IDatabaseApiContainer
{
    [CanBeNull]
    IDatabaseApi FindDatabaseApi([NotNull] string key);

    void AddDatabaseApi([NotNull] string key, [NotNull] IDatabaseApi api);

    [NotNull]
    IDatabaseApi GetOrAddDatabaseApi([NotNull] string key, [NotNull] Func<IDatabaseApi> factory);
}