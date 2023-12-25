using System;

namespace Fake.UnitOfWork;

/// <summary>
/// 数据库api容器
/// </summary>
public interface IDatabaseApiContainer : IServiceProviderAccessor
{
    IDatabaseApi? FindDatabaseApi(string key);

    void AddDatabaseApi(string key, IDatabaseApi api);

    IDatabaseApi GetOrAddDatabaseApi(string key, Func<IDatabaseApi> factory);
}