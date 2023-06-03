using Domain.Aggregates.BuyerAggregate;
using Domain.Aggregates.OrderAggregate;
using Fake.Domain.Repositories.EntityFrameWorkCore;

namespace Repositories;

public class BuyerEfCoreEfCoreRepository: EfCoreEfCoreRepository<OrderingContext, Buyer>, IBuyerRepository
{
    public Buyer Add(Buyer buyer)
    {
        throw new NotImplementedException();
    }

    public Buyer Update(Buyer buyer)
    {
        throw new NotImplementedException();
    }

    public Task<Buyer> FindAsync(Guid buyerIdentityGuid)
    {
        throw new NotImplementedException();
    }

    public Task<Buyer> FindByIdAsync(string id)
    {
        throw new NotImplementedException();
    }
}