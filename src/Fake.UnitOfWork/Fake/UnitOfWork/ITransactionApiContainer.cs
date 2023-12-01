using System;
using JetBrains.Annotations;

namespace Fake.UnitOfWork;

public interface ITransactionApiContainer
{
    [CanBeNull]
    ITransactionApi FindTransactionApi(string key);

    void AddTransactionApi(string key, ITransactionApi api);


    ITransactionApi GetOrAddTransactionApi(string key, Func<ITransactionApi> factory);
}