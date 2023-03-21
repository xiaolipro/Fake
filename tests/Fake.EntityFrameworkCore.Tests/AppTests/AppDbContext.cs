using Domain.Aggregates.OrderAggregate;
using Fake.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Fake.EntityFrameworkCore.Tests.AppTests;

public class AppDbContext:FakeDbContext<AppDbContext>
{
    public DbSet<Order> Orders { get; set; }
    
    public AppDbContext(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}