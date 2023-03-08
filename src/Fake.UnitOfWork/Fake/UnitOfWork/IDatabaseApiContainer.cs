using System;
using Fake.DependencyInjection;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;

/// <summary>
/// 数据库api容器
/// </summary>
public interface IDatabaseApiContainer : IServiceProviderAccessor
{
    [CanBeNull]
    IDatabaseApi FindDatabaseApi([NotNull] string key);

    void AddDatabaseApi([NotNull] string key, [NotNull] IDatabaseApi api);

    [NotNull]
    IDatabaseApi GetOrAddDatabaseApi([NotNull] string key, [NotNull] Func<IDatabaseApi> factory);
}