using System.Threading.Tasks;
using Domain.Aggregates.BuyerAggregate;
using Fake.Domain.Repositories;

namespace Domain.Aggregates.BuyerAggregate;
//This is just the RepositoryContracts or Interface defined at the Domain Layer
//as requisite for the Buyer Aggregate

public interface IBuyerRepository : IRepository<Buyer>
{
    Buyer Add(Buyer buyer);
    Buyer Update(Buyer buyer);
    Task<Buyer> FindAsync(string buyerIdentityGuid);
    Task<Buyer> FindByIdAsync(string id);
}

