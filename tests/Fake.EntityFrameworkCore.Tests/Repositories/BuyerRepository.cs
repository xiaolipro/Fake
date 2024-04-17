using Domain.Aggregates.BuyerAggregate;
using Fake.DomainDrivenDesign.Repositories.EntityFrameWorkCore;

namespace Fake.EntityFrameworkCore.Tests.Repositories;

public class BuyerRepository : EfCoreRepository<OrderingContext, Buyer>, IBuyerRepository
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